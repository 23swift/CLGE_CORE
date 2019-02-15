// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdsServer.Data;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdsServer {
    public class SeedData {
        public static void EnsureSeedData (string connectionString) {
            var services = new ServiceCollection ();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; 
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlite (connectionString));
            

            services.AddIdentity<ApplicationUser, ApplicationRole> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ()
                .AddDefaultTokenProviders ();

            using (var serviceProvider = services.BuildServiceProvider ()) {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory> ().CreateScope ()) {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext> ();
                    context.Database.Migrate ();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>> ();
                    
                   
                    var alice = userMgr.FindByNameAsync ("alice").Result;
                    if (alice == null) {
                        alice = new ApplicationUser {
                        UserName = "alice"
                        };
                        var result = userMgr.CreateAsync (alice, "Pass123$").Result;
                        if (!result.Succeeded) {
                            throw new Exception (result.Errors.First ().Description);
                        }

                        result = userMgr.AddClaimsAsync (alice, new Claim[] {
                            new Claim (JwtClaimTypes.Name, "Alice Smith"),
                                new Claim (JwtClaimTypes.GivenName, "Alice"),
                                new Claim (JwtClaimTypes.FamilyName, "Smith"),
                                new Claim (JwtClaimTypes.Email, "AliceSmith@email.com"),
                                new Claim (JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim (JwtClaimTypes.WebSite, "http://alice.com"),
                                // new Claim(JwtClaimTypes.Role, "AO Checker"),
                                new Claim (JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                                new Claim ("system", "map"),
                                new Claim ("group", "AO"),
                                new Claim ("rank", "C1"),
                                new Claim ("userId", alice.UserName)
                        }).Result;
                        if (!result.Succeeded) {
                            throw new Exception (result.Errors.First ().Description);
                        }
                        Console.WriteLine ("alice created");
                    } else {
                        Console.WriteLine ("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync ("bob").Result;
                    if (bob == null) {
                        bob = new ApplicationUser {
                        UserName = "bob"
                        };
                        var result = userMgr.CreateAsync (bob, "Pass123$").Result;
                        if (!result.Succeeded) {
                            throw new Exception (result.Errors.First ().Description);
                        }

                        result = userMgr.AddClaimsAsync (bob, new Claim[] {
                            new Claim (JwtClaimTypes.Name, "Bob Smith"),
                                new Claim (JwtClaimTypes.GivenName, "Bob"),
                                new Claim (JwtClaimTypes.FamilyName, "Smith"),
                                new Claim (JwtClaimTypes.Email, "BobSmith@email.com"),
                                new Claim (JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim (JwtClaimTypes.WebSite, "http://bob.com"),
                                // new Claim(JwtClaimTypes.Role, "AO Checker"),
                                new Claim (JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                                new Claim ("system", "map"),
                                new Claim ("group", "AO"),
                                new Claim ("rank", "C1"),
                                new Claim ("userId", bob.UserName)

                        }).Result;
                        if (!result.Succeeded) {
                            throw new Exception (result.Errors.First ().Description);
                        }
                        Console.WriteLine ("bob created");
                    } else {
                        Console.WriteLine ("bob already exists");
                    }

                    ///Roles
                     
                   
                    //  roleMgr.AddClaimAsync (role, new Claim ( "dashboard", "aoDasboard"));
                    //  roleMgr.AddClaimAsync (role, new Claim ("route", "newAff"));
                    //  roleMgr.AddClaimAsync (role, new Claim ("route", "branchAf"));
                }
            }
        }
    }
}