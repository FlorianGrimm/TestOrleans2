namespace TestOrleans2.TestExtensions;

[CollectionDefinition("DefaultClusterFixture")]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class DefaultClusterFixture : BaseClusterFixture {
    public DefaultClusterFixture() {
    }

    public override void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
        TestDefaultConfiguration.ConfigureTestCluster(builder);
        builder.AddSiloBuilderConfigurator<SiloHostConfigurator>();
    }
    

    public class SiloHostConfigurator : ISiloConfigurator {
        public void Configure(ISiloBuilder hostBuilder) {

            hostBuilder.ConfigureServices((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
                var startup = new TestOrleans2.WebApp.Startup(hostBuilderContext.Configuration);

                startup.AddAppServicesWithRegistrator(services);
                startup.AddAppAuthenticationOptions(services);
                startup.AddAppOptions(services);
                startup.AddAppRequestLog(services);
                
            });


            hostBuilder
                .UseInMemoryReminderService()
                .AddMemoryGrainStorageAsDefault()
                .AddMemoryGrainStorage("MemoryStore");
        }
    }
}
