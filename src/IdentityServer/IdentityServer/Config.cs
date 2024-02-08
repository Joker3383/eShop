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
                new ApiScope("product", "ProductController"),
                new ApiScope("basket","BasketAPI"),
                new ApiScope("order", "OrderAPI")
            };

        public static IEnumerable<Client> Clients =>
            new []
            {
                new Client
                {
                    ClientId = "basket",
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    ClientSecrets =
                    {
                        new Secret("basketAPI".Sha256())
                    },
                    
                    AllowedScopes = { "product" }
                },
                new Client
                {
                    ClientId = "order",
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    ClientSecrets =
                    {
                        new Secret("order".Sha256())
                    },
                    
                    AllowedScopes = { "basket" }
                },
                new Client()
                {
                    ClientId = "order_swaggerui",
                    ClientName = "Order Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:5003/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/swagger/" },

                    AllowedScopes =
                    {
                        "order"
                    }
                },
                new Client()
                {
                    ClientId = "basket_swaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:5002/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:5002/swagger/" },

                    AllowedScopes =
                    {
                        "basket"
                    }
                },
                new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:5000/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:5000/swagger/" },

                    AllowedScopes =
                    {
                        "catalog", "product"
                    }
                },
                new Client
                {
                    ClientId = "mvc-client",
                    ClientName = "MVC Client",
                    ClientSecrets = { new Secret("mvc-client-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:5050/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5050/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "profile",
                        "product",
                        "basket",
                        "order"
                    },
                }
            };
    }
}