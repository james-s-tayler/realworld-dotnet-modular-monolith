using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;
using Xunit;

namespace Conduit.Core.Tests.Unit
{
    public class RefitPollyTests
    {
        public interface IHttpStatus
        {
            //this one leverages a DelegatingHandler to populate the Polly.Context instead,
            //which is a better and much cleaner option if the content of the Polly.Context doesn't _require_ dynamic content only known at runtime
            [Get("/{statusCode}")]
            Task<ApiResponse<string>> GetStatusCodeWithPollyContextInjected([Query] int statusCode);
        }
        
        private const string Logger = nameof(Logger);
        
        [Fact]
        public async Task GivenPollyContextInjectedByDelegatingHandler_WhenRetryPolicyInvoked_ThenLogsRetryCount()
        {
            //arrange
            var services = new ServiceCollection();

            services.AddSingleton<ILogger, TestLogger>();
            services.AddTransient<PollyContextInjectingDelegatingHandler>();
            services.AddRefitClient<IHttpStatus>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("http://httpstat.us/");
                })
                //order matters here
                .AddHttpMessageHandler<PollyContextInjectingDelegatingHandler>()
                .AddTransientHttpErrorPolicy(builder => builder.RetryAsync((result, retryCount, context) =>
                {
                    var logger = context[Logger] as ILogger;
                    logger!.LogInformation($"retry_count={retryCount}");
                }));
            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger>();
            var refitClient = serviceProvider.GetService<IHttpStatus>();

            //act
            var result = await refitClient.GetStatusCodeWithPollyContextInjected(StatusCodes.Status503ServiceUnavailable);
            
            //assert
            result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
            var testLogger = logger as TestLogger;
            testLogger.Logs.Count.Should().Be(1);
            testLogger.Logs.Should().Contain("retry_count=1");
        }

        public class PollyContextInjectingDelegatingHandler : DelegatingHandler
        {
            //realistically you would probably use a logger factory or ILogger<T> here
            //but just showing how it can be accomplished through DI and a DelegatingHandler
            private readonly ILogger _logger;
            
            public PollyContextInjectingDelegatingHandler(ILogger logger)
            {
                _logger = logger;
            }
            
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                var pollyContext = new Polly.Context();
                pollyContext.Add(Logger, _logger);
                request.SetPolicyExecutionContext(pollyContext);
                
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
        
        public class TestLogger : ILogger
        {
            public List<string> Logs { get; } = new ();
            
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Logs.Add(formatter.Invoke(state, exception));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return default;
            }
        }
    }
}