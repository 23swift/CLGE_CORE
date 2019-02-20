using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdsServer.Models;
using IdsServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityModel;
using System.Reflection;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdsServer
{
     public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; 
            var connectionString=Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString)
                // options.UseSqlServer(connectionString)
                );

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                // .AddInMemoryIdentityResources(Config.GetIdentityResources())
                // .AddInMemoryApiResources(Config.GetApis())
                // .AddInMemoryClients(Config.GetClients())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = configBuilder =>
                        configBuilder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly)
                            
                            );
                            
                            
                            
                }) 
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = operatioBuilder =>
                        operatioBuilder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
                services.AddTransient<IProfileService, ProfileService>();

                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "clge";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("access.profile");
                    options.Scope.Add("role");
                   
                    options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = JwtClaimTypes.Name,
                            RoleClaimType = JwtClaimTypes.Role
                            //NameClaimType = ClaimTypes.Name,
                            //RoleClaimType = ClaimTypes.Role
                        };
                // options.ClaimActions.MapUniqueJsonKey("role", "role");
                options.ClaimActions.MapJsonKey("role", "role");
                options.ClaimActions.MapJsonKey("Permission", "Permission");
               
                    // options.ClaimActions.MapJsonKey("website", "website");
                    
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseAuthentication();
           
            app.UseCookiePolicy();
                                app.Use(async (context, next) =>
                                {
                                    if (!context.User.Identity.IsAuthenticated)
                                    {
                                        await context.ChallengeAsync("oidc",
                                        new AuthenticationProperties { RedirectUri = "/" });

                                        // await context.ChallengeAsync("oidc");
                                    }
                                    else
                                    {
                                        await next();
                                    }
                    });
            app.UseMvcWithDefaultRoute();
        }
    }
}
