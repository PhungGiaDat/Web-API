using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Web_API.Security.Interfaces;

namespace Web_API.Security.Handlers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApiKeyService _apiKeyService;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiKeyService apiKeyService)
            : base(options, logger, encoder, clock)
        {
            _apiKeyService = apiKeyService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(ApiKeyHeaderName))
            {
                return AuthenticateResult.Fail("API Key missing");
            }

            var apiKey = Request.Headers[ApiKeyHeaderName].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail("API Key empty");
            }

            var isValid = await _apiKeyService.ValidateApiKeyAsync(apiKey);
            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid API Key");
            }

            var shopId = await _apiKeyService.GetShopIdFromApiKeyAsync(apiKey);
            if (string.IsNullOrEmpty(shopId))
            {
                return AuthenticateResult.Fail("Shop not found for API Key");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, shopId),
                new Claim(ClaimTypes.Role, "Shop"),
                new Claim("ShopId", shopId)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        // Additional options if needed
    }
}