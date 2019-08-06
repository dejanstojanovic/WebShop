using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Web.Mvc
{
    public class AutomaticCookieTokenEvents : CookieAuthenticationEvents
    {

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly TokenEndpointService _tokenEndpointService;
        readonly ILogger<AutomaticCookieTokenEvents> _logger;
        private static readonly ConcurrentDictionary<string, bool> _pendingRefreshTokenRequests = 
            new ConcurrentDictionary<string, bool>();

        public AutomaticCookieTokenEvents(
            IHttpContextAccessor httpContextAccessor, 
            TokenEndpointService tokenEndpointService,
            ILogger<AutomaticCookieTokenEvents> logger)
        {
            _tokenEndpointService = tokenEndpointService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {

            var tokens = context.Properties.GetTokens();
            if (tokens == null && !tokens.Any())
                return;

            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);

            if (refreshToken == null)
                return;

            var expiresAt = tokens.SingleOrDefault(t => t.Name.Equals("expires_at"));

            if (expiresAt == null)
                return;

            var dateTimeExpires = DateTimeOffset.Parse(expiresAt.Value, CultureInfo.InvariantCulture);
            var minutesLeft = dateTimeExpires.Subtract(new DateTimeOffset(DateTime.UtcNow)).TotalMinutes;
            if (minutesLeft < 5 && _pendingRefreshTokenRequests.TryAdd(refreshToken.Value, true))
            {
                try
                {
                    var response = await _tokenEndpointService.RefreshTokenAsync(refreshToken.Value);

                    if (!response.IsError)
                    {
                        context.Properties.UpdateTokenValue("access_token", response.AccessToken);
                        context.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                        var newExpiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
                        context.Properties.UpdateTokenValue("expires_at", newExpiresAt.ToString("o", CultureInfo.InvariantCulture));

                        await context.HttpContext.SignInAsync(context.Principal, context.Properties);

                    }
                }
                finally
                {
                    _pendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);
                }
            }
        }

        public override async Task SigningOut(CookieSigningOutContext context)
        {

            var result = await context.HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                _logger.LogDebug("Can't find cookie for default scheme. Might have been deleted already.");
                return;
            }

            var tokens = result.Properties.GetTokens();
            if (tokens == null || !tokens.Any())
            {
                _logger.LogDebug("No tokens found in cookie properties. SaveTokens must be enabled for automatic token revocation.");
                return;
            }

            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                _logger.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return;
            }

            var response = await _tokenEndpointService.RevokeTokenAsync(refreshToken.Value);

            if (response.IsError)
            {
                _logger.LogWarning("Error revoking token: {error}", response.Error);
                return;
            }
        }


    }
}
