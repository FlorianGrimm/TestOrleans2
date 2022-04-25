using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Replacement.OData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
#if false

Scaffold-DbContext "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -Namespace Replacement.OData - NoPluralize - OutputDir Record

dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef

dotnet user-secrets set Database "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;"

dotnet ef dbcontext scaffold Name=Database Microsoft.EntityFrameworkCore.SqlServer --namespace Replacement.OData --no-pluralize --output-dir OData --force

#endif