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
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                
                new IdentityResource{
                    Name="group_access",
                    DisplayName="Group Access",
                    UserClaims={"group_code",JwtClaimTypes.Role,"group_name","route_access"}
                    

                    
                }
                // new IdentityResource{
                //     Name="role",
                //     DisplayName="User Role",
                //     UserClaims={JwtClaimTypes.}
                // }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "map",
                    ClientName = "Merchant Acquiring System",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "https://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },
                    FrontChannelLogoutUri= "https://localhost:5003/signout-oidc",
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                        "api1","group_access"
                    },
                    // RequireConsent=false,
                    AllowOfflineAccess = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.WebSite, "https://alice.com"),
                        new Claim(JwtClaimTypes.Email, "alice@alice.com"),
                        new Claim(JwtClaimTypes.Role,"Admin"),
                        new Claim(JwtClaimTypes.Role,"Manager"),
                        new Claim(JwtClaimTypes.Role,"Supervisor"),
                        new Claim("api1", "api1"),
                        new Claim("group_code", "ao"),
                        new Claim("group_name", "Account Officer"),
                        new Claim("route_access", "{'newAffEncode','newAffCheker'}")

                       

                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com"),
                          new Claim(JwtClaimTypes.Email, "bob@bob.com")
                    }
                }
            };
        }
    }
}