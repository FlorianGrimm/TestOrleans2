namespace Replacement.TestExtensions;

[ExcludeFromCodeCoverage]
public abstract class  TestClusterTestingBase : OrleansTestingBase {
    public BaseClusterFixture Fixture { get; private set; }
    public TestCluster HostedCluster => this.Fixture.HostedCluster ?? throw new InvalidOperationException("this.Fixture.HostedCluster is null");

    public IGrainFactory GrainFactory => this.HostedCluster.GrainFactory;

    public IClusterClient Client => this.HostedCluster.Client;

    public ILogger Logger => this.Client.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(this.GetType().Namespace ?? "Application");

    protected TestClusterTestingBase(
        ITestOutputHelper output,
        BaseClusterFixture fixture
        ) {
        fixture.TestOutputHelper = output;
        this.Fixture = fixture;        
    }
}
