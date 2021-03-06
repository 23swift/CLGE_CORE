// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdsServer
{
     public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
             var accessProfile = new IdentityResource(
                name: "access.profile",
                displayName: "Access Profile",
                claimTypes: new[] { "system", "group","role","rank","userId","access"});
                
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                accessProfile,
                new IdentityResource {
                Name = "role", DisplayName="User Roles",
                UserClaims = new List<string> {"role"},Required=true
                }
                ,
                new IdentityResource {
                Name = "access", DisplayName="User Access",
                UserClaims = new List<string> {"access"},Required=true
                }
                 
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "clge",
                    ClientName = "CGL Exchange",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { "http://localhost:5000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },
                    

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                        
                        "api1","access.profile","role","token"
                    },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                },
                // // OpenID Connect hybrid flow client (MVC)
                new Client
                {
                    ClientId = "map",
                    ClientName = "Merchant Acquiring System",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { "https://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                        
                        "api1","access.profile","role","token"
                    },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
        }
    }
}