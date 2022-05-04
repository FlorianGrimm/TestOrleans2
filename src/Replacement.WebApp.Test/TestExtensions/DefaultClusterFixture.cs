namespace Replacement.TestExtensions;

[CollectionDefinition("DefaultClusterFixture")]
public class DefaultClusterFixture : Xunit.IAsyncLifetime {
    static DefaultClusterFixture() {
        TestDefaultConfiguration.InitializeDefaults();
    }
    public DefaultClusterFixture() {
    }

    public TestCluster? HostedCluster { get; private set; }

    public IGrainFactory? GrainFactory => this.HostedCluster?.GrainFactory;

    public IClusterClient? Client => this.HostedCluster?.Client;

    public ILogger? Logger { get; private set; }

    public ITestOutputHelper? TestOutputHelper;

    public virtual async Task InitializeAsync() {
        var builder = new TestClusterBuilder();

        TestDefaultConfiguration.ConfigureTestCluster(builder);
        builder.AddSiloBuilderConfigurator<SiloHostConfigurator>();

        var testCluster = builder.Build();
        if (testCluster.Primary == null) {
            await testCluster.DeployAsync().ConfigureAwait(false);
        }

        this.HostedCluster = testCluster;
        
        this.Logger = testCluster.Client.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Application");
    }

    public virtual async Task DisposeAsync() {
        var cluster = this.HostedCluster;
        if (cluster is null) return;

        try {
            await cluster.StopAllSilosAsync().ConfigureAwait(false);
        } finally {
            await cluster.DisposeAsync().ConfigureAwait(false);
        }
    }

    public class SiloHostConfigurator : ISiloConfigurator {
        public void Configure(ISiloBuilder hostBuilder) {
            //hostBuilder.ConfigureServices(
            //    (Microsoft.Extensions.Hosting.HostBuilderContext ctxt, IServiceCollection services) => { 
            //    }
            //    );
            hostBuilder
                .UseInMemoryReminderService()
                .AddMemoryGrainStorageAsDefault()
                .AddMemoryGrainStorage("MemoryStore");
        }
    }
}
