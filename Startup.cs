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
namespace IdsServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; 
        var connectionString=Configuration.GetConnectionString("DefaultConnection");
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
            
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddUserStore<ApplicationUserStore>();
                
            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            // .AddInMemoryClients(Config.GetClients())
            // .AddTestUsers(Config.GetUsers())
            .AddAspNetIdentity<ApplicationUser>()
            // .AddInMemoryIdentityResources(Config.GetIdentityResources())
            // .AddInMemoryApiResources(Config.GetApiResources());
             .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlite(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
