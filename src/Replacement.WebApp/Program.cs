namespace Replacement.WebApp;

[ExcludeFromCodeCoverage]
public static class Program {
    public static async Task<int> Main(string[] args) {
        try {
            var hostBuilder = CreateHostBuilder(args);
            if (global::Replacement.WebApp.Swagger.SwaggerGenerator.Generate(
                args: args,
                hostBuilder: hostBuilder,
                swaggerOptions: Startup.GetSwaggerOptions(),
                configureForSwaggerGeneration: hostBuilder => {
                    hostBuilder.UseEnvironment("SwaggerGenerator");
                })) {
                return 0;
            } else {
                var host = hostBuilder.Build();
                await host.RunAsync(default);
                return 0;
            }
        } catch (Exception error) {
            Replacement.WebApp.Utility.ExceptionExtensions.WriteError(error);
            return 1;
        }
    }

    public static Microsoft.Extensions.Hosting.IHostBuilder CreateHostBuilder(string[] args) {
        var hostBuilder = Host.CreateDefaultBuilder(args);
        //hostBuilder.ConfigureServices(services => {
        //    services.AddTransient<IDBContext, DBContext>();
        //});
        hostBuilder.UseOrleans((ctx, builder) => {
            builder.UseLocalhostClustering()
            //.Configure<Orleans.Configuration.SchedulingOptions>(o => { 
            //    o.TurnWarningLengthThreshold= TimeSpan.FromMilliseconds(2000);
            //})
            ;
            //TurnWarningLengthThreshold

        });
        hostBuilder.ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
        });
        return hostBuilder;
    }
}
