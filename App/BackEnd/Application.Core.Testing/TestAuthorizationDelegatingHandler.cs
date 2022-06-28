using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Application.Core.Testing
{
    public class TestAuthorizationDelegatingHandler: DelegatingHandler
    {
        private readonly string _token;
        private readonly string _scheme;

        public TestAuthorizationDelegatingHandler([NotNull] string token, string scheme = "Bearer")
        {
            _token = token;
            _scheme = scheme;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(_scheme, _token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}