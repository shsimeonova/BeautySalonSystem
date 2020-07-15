﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("role", new[] { "role" })
           };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("ms"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "BeautySalonSystem.UI",
                    ClientSecrets = { new Secret("".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenLifetime = 120,
                    IdentityTokenLifetime = 120,

                    RedirectUris = { "https://localhost:4009/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:4009/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:4009/signout-callback-oidc" },

                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2", "role", "ms" }
                }
                // new Client
                // {
                //     ClientId = "BeautySalonSystem.Products",
                //     ClientSecrets = { new Secret("Valt1n3r_01234567".Sha256()) },
                //     
                //     AllowedGrantTypes = GrantTypes.Hybrid,
                //     AccessTokenLifetime = 60 * 2,
                //     IdentityTokenLifetime = 60 * 2,
                //
                //     RedirectUris = { "https://localhost:4009/signin-oidc" },
                //     FrontChannelLogoutUri = "https://localhost:4009/signout-oidc",
                //     PostLogoutRedirectUris = { "https://localhost:4009/signout-callback-oidc" },
                //
                //     AllowOfflineAccess = false,
                //     AllowedScopes = { "openid", "profile", "scope2", "role" }
                // },
            };
    }
}