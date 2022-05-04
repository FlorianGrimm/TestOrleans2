namespace Replacement.TestExtensions;

/// <summary>
/// Base class that ensures a silo cluster is started with the default configuration, and avoids restarting it if the previous test used the same default base.
/// </summary>
[Collection("DefaultCluster")]
public abstract class HostedTestClusterEnsureDefaultStarted : TestClusterTestingBase {
    protected HostedTestClusterEnsureDefaultStarted(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) :base (output, fixture){
    }
}
