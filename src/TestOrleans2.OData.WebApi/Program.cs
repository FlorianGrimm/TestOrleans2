namespace Replacement.OData;
public class Program {
    public static void Main(string[] args) {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
            });
    }
}

#if false

Scaffold-DbContext "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -Namespace Replacement.OData - NoPluralize - OutputDir Record

dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef

dotnet user-secrets set Database "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;"

dotnet user-secrets set Database "Data Source=parado.dev.solvin.local;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;"

dotnet ef dbcontext scaffold Name=Database Microsoft.EntityFrameworkCore.SqlServer --namespace Replacement.OData --no-pluralize --output-dir OData --force


curl -X 'GET' \
  'https://localhost:44302/api/Users' \
  -H 'accept: application/json;odata.metadata=minimal;odata.streaming=true'

#endif