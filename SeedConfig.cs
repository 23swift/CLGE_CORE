using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdsServer;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IdsServer {
    public class SeedConfig {
        static RoleManager<ApplicationRole> roleMgr;
        public static void EnsureSeedData (IServiceProvider serviceProvider) {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory> ().CreateScope ()) {
                using (var context = scope.ServiceProvider.GetService<IConfigurationDbContext> ()) {
                    roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>> ();
                    EnsureSeedData (context);

                }
            }
        }

        private static void EnsureSeedData (IConfigurationDbContext context) {
            Console.WriteLine ("Seeding database...");

            if (!context.Clients.Any ()) {
                Console.WriteLine ("Clients being populated");
                foreach (var client in Config.GetClients ()) {
                    context.Clients.Add (client.ToEntity ());

                   

                }
                context.SaveChanges ();
               

                     
                    
            } else {
                Console.WriteLine ("Clients already populated");
            }

            if (!context.IdentityResources.Any ()) {
                Console.WriteLine ("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources ()) {
                    context.IdentityResources.Add (resource.ToEntity ());
                }
                context.SaveChanges ();
            } else {
                Console.WriteLine ("IdentityResources already populated");
            }

            if (!context.ApiResources.Any ()) {
                Console.WriteLine ("ApiResources being populated");
                foreach (var resource in Config.GetApis ()) {
                    context.ApiResources.Add (resource.ToEntity ());
                }
                context.SaveChanges ();
            } else {
                Console.WriteLine ("ApiResources already populated");
            }

            Console.WriteLine ("Done seeding database.");
            Console.WriteLine ();
        }
    }
}