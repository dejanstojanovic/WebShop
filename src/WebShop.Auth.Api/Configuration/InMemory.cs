using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebShop.Auth.Api.Configuration
{
    public class InMemory
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[] {
                new ApiResource("webshop.users.api", "WebShop Users API")
                {
                    UserClaims = new [] { "email", "userid" }
                }
            };
        }
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
        public static IEnumerable<Client> Clients()
        {
            return new[] {
                new Client
                {
                    ClientId = "webshop.app",
                    ClientName="WebShop Backend Application",
                    Description="Secure client using ResourceOwnerPassword flow",
                    ClientSecrets = new [] { new Secret("5aKtv2wvyP".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] {
                        "webshop.users.api"
                    }
                },
                new Client
                {
                    ClientId = "webshop.js",
                    ClientName="WebShop JavaScrip Web Application",
                    Description="Unsecure static content client",
                    ClientSecrets = new [] { new Secret("5aKtv2wvyP".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "webshop.users.api"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "https://localhost:5004/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5004/signout-callback-oidc" },
                },
                new Client
                {
                    ClientId = "webshop.web",
                    ClientName="WebShop Web Application",
                    Description="Web application client with back-channel",
                    ClientSecrets = new [] { new Secret("5aKtv2wvyP".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "webshop.users.api"
                    },
                    AccessTokenLifetime = (int)TimeSpan.FromHours(1).TotalSeconds,
                    AuthorizationCodeLifetime = (int)TimeSpan.FromMinutes(5).TotalSeconds,
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new [] {
                        "https://localhost:5004/signin-oidc"
                    },
                    PostLogoutRedirectUris = {
                        "https://localhost:5004/signout-callback-oidc"
                    },
                }
            };
        }
    }
}
