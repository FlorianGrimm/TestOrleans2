namespace Replacement.Repository.Grains;

[Collection("DefaultClusterFixture")]
public class ProjectGrainTest 
    : HostedTestClusterEnsureDefaultStarted
    , IClassFixture<DefaultClusterFixture> {
    public ProjectGrainTest(ITestOutputHelper output, DefaultClusterFixture fixture) : base(output, fixture) {
    }

    [Fact]
    public void Test1() {

    }


    [Fact]
    public void Test2() {

    }
}