namespace Replacement.WebApp.Controllers;

[Collection("DefaultClusterFixture")]
public class ProjectControllerTests : HostedTestClusterEnsureDefaultStarted {

    public ProjectControllerTests(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) : base(output, fixture) {

    }
}
