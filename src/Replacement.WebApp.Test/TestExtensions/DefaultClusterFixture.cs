namespace Replacement.TestExtensions;

[CollectionDefinition("DefaultClusterFixture")]
public class DefaultClusterFixture : BaseClusterFixture {
    public DefaultClusterFixture() {
    }

    public override void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
        TestDefaultConfiguration.ConfigureTestCluster(builder);
        builder.AddSiloBuilderConfigurator<SiloHostConfigurator>();
    }

    public class SiloHostConfigurator : ISiloConfigurator {
        public void Configure(ISiloBuilder hostBuilder) {
            hostBuilder.ConfigureServices(
               (Microsoft.Extensions.Hosting.HostBuilderContext ctxt, IServiceCollection services) => {
                   var startup = new Replacement.WebApp.Startup(ctxt.Configuration);
                   startup.AddAppServicesWithRegistrator(services);
                   startup.AddAppOptions(services);
               }
               );
            hostBuilder
                .UseInMemoryReminderService()
                .AddMemoryGrainStorageAsDefault()
                .AddMemoryGrainStorage("MemoryStore");
        }
    }
}
