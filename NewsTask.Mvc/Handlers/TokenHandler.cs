using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace NewsTask.Mvc.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor accessor;

        public TokenHandler(IHttpContextAccessor accessor) => this.accessor = accessor;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //get the token
            var accessToken = await accessor.HttpContext.GetTokenAsync("JWT_Token");
            //add header
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            //continue down stream request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
