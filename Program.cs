﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var seed = args.Any(x => x == "/seed");
            if (seed) args = args.Except(new[] { "/seed" }).ToArray();

            // CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();
             if (seed)
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");
                SeedData.EnsureSeedData(connectionString);
                SeedConfig.EnsureSeedData(host.Services);
                return;
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
