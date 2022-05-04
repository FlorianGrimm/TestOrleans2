namespace Replacement.TestExtensions;

public class BaseClusterFixture : Xunit.IAsyncLifetime {
    public TestCluster? HostedCluster { get; private set; }

    public IGrainFactory? GrainFactory => this.HostedCluster?.GrainFactory;

    public IClusterClient? Client => this.HostedCluster?.Client;

    public ILogger? Logger { get; private set; }

    public ITestOutputHelper? TestOutputHelper;

    public BaseClusterFixture() {
    }
    public virtual void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
    }

    public virtual async Task InitializeAsync() {
        var builder = new TestClusterBuilder();
        this.ConfigureTestClusterBuilder(builder);

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

}
