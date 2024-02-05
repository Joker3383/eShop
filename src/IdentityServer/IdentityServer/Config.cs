// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new []
            {
                new ApiScope("catalog", "BffController"),
                new ApiScope("product", "ProductController")
            };

        public static IEnumerable<Client> Clients =>
            new []
            {
                new Client
                {
                    ClientId = "mvc-client",
                    ClientName = "MVC Client",
                    ClientSecrets = { new Secret("mvc-client-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5050/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5050/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "profile",
                        "product"
                    },
                },
                new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "https://localhost:7000/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "https://localhost:7000/swagger/" },

                    AllowedScopes =
                    {
                        "catalog", "product"
                    }
                },
               
            };
    }
}