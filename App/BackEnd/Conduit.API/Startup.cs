using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using App.Core.Exceptions;
using App.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Conduit.API.Filters;
using Conduit.API.OpenApi;
using Conduit.API.Formatters;
using Conduit.API.Models;
using Dapper.Logging;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Npgsql;
using OpenTelemetry.Trace;
using Serilog;

namespace Conduit.API
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// The application configuration.
        /// </summary>
        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var authScheme = "Token"; //JwtBearerDefaults.AuthenticationScheme is normally the default but Conduit uses "Token"
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = authScheme;
                    options.DefaultChallengeScheme = authScheme;
                    options.DefaultScheme = authScheme;
                })
                .AddScheme<JwtBearerOptions, JwtBearerHandler>(authScheme, options =>
                {
                    options.Challenge = authScheme;
                    options.Audience = Configuration["JwtSettings:ValidAudience"];
                    options.Authority = Configuration["JwtSettings:ValidIssuer"];
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEventsLogger(new CustomSchemeJwtBearerEvents(authScheme));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtSettings:ValidAudience"],
                        ValidIssuer = Configuration["JwtSettings:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"]))
                    };
                });
            services.AddAuthorization();

            // Add framework services.
            services
                // Don't need the full MVC stack for an API, see https://andrewlock.net/comparing-startup-between-the-asp-net-core-3-templates/
                .AddControllers(options =>
                {
                    options.InputFormatters.Insert(0, new InputFormatterStream());
                });

            //caching
            /*var redisHost = _configuration["Redis:Host"] ?? throw new ArgumentNullException("Redis:Host");
            var redisPort = _configuration["Redis:Port"] ?? throw new ArgumentNullException("Redis:Port");
            var redisAdmin = _configuration["Redis:AllowAdmin"] ?? throw new ArgumentNullException("Redis:AllowAdmin");

            var redisConnectionString = $"{redisHost}:{redisPort},allowAdmin={redisAdmin}";
            var redisClientConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddStackExchangeRedisCache(options =>
            {
                //providing our own instance of ConnectionMultiplexer allows us to instrument StackExchangeRedisCache via OpenTelemetry
                options.ConnectionMultiplexerFactory = () => Task.FromResult((IConnectionMultiplexer)redisClientConnection);
            });
            services.AddSingleton<IConnectionMultiplexer>(redisClientConnection);
            services.AddSingleton<IDatabaseAsync>(provider => //IDatabaseAsync provides raw access to Redis when we need it
            {
                var connectionMultiplexer = provider.GetService<IConnectionMultiplexer>();
                return connectionMultiplexer.GetDatabase(0);
            });
            services.AddSingleton<IRedisCache, RedisCache>();*/

            //tracing
            services.AddOpenTelemetryTracing(
                builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()                     //trace inbound http requests
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.RecordException = true;
                            options.SetDbStatementForText = true;
                        })
                        .AddHttpClientInstrumentation()                     //trace outbound http requests
                        .AddJaegerExporter(options =>                       //export traces to jaeger container
                            options.AgentHost = "jaeger");
                });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.0.0", new OpenApiInfo
                    {
                        Title = "Conduit API",
                        Description = "Conduit API (ASP.NET Core 5.0)",
                        TermsOfService = new Uri("https://github.com/openapitools/openapi-generator"),
                        Contact = new OpenApiContact
                        {
                            Name = "RealWorld",
                            Url = new Uri("https://realworld.io"),
                            Email = ""
                        },
                        License = new OpenApiLicense
                        {
                            Name = "NoLicense",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        Version = "1.0.0",
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(true));
                    //c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");
                    c.EnableAnnotations();

                    c.AddSecurityDefinition(authScheme, new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header",
                        In = ParameterLocation.Header,
                        Name = HeaderNames.Authorization,
                        Scheme = "bearer",
                        Type = SecuritySchemeType.ApiKey, //normally we use SecuritySchemeType.Http but that hardcodes "Bearer" but Conduit needs "Token"
                        BearerFormat = authScheme + " {token}" //this doesn't affect anything, it's essentially just documentation
                    });
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                    // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
            
            // This is what I would prefer to do, but it doesn't match the spec,
            // so just leaving it here for reference on how to do this.
            /*services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "Validation Failed",
                        Type = "https://tools.ietf.org/html/rfc4918#section-11.2"
                    };

                    return new UnprocessableEntityObjectResult(problemDetails);
                };
            });*/
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new GenericErrorModel
                    {
                        Errors = new GenericErrorModelErrors
                        {
                            Body = new List<string>()
                        }
                    };
                    
                    foreach (var error in context.ModelState
                        .Where(e => e.Value.Errors.Count > 0))
                    {
                        var key = error.Key;
                        if (key.Contains("."))
                        {
                            var objectGraphKeys = key.Split('.');
                            key = objectGraphKeys.Last().ToLower();
                        }
                        errors.Errors.Body.Add($"{key}: {string.Join(";", error.Value.Errors.Select(e => e.ErrorMessage).ToArray())}");
                    }

                    return new UnprocessableEntityObjectResult(errors);
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if ( env.IsProduction() )
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseSwagger(c =>
                    {
                        c.RouteTemplate = "openapi/{documentName}/openapi.json";
                    })
                    .UseSwaggerUI(c =>
                    {
                        // set route prefix to openapi, e.g. http://localhost:8080/openapi/index.html
                        c.RoutePrefix = "openapi";
                        //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
                        c.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Conduit API");
                    });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if ( contextFeature != null )
                    {

                        var errors = new GenericErrorModel
                        {
                            Errors = new GenericErrorModelErrors
                            {
                                Body = contextFeature.Error.GetErrorMessages()
                            }
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errors));
                    }
                });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization();
            });
        }

        /*static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
        {
            var maxRetryAttempts = configuration.GetValue<int>("HttpClientConfig:MaxRetryAttempts");

            //retry with exponential backoff
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(maxRetryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
        }*/
    }
}
