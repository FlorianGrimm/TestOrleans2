namespace TestOrleans2.WebApp.Controllers;

[Collection("DefaultClusterFixture")]
public class MeControllerTests : HostedTestClusterEnsureDefaultStarted {

    public MeControllerTests(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) : base(output, fixture) {

    }

    [Fact]
    public async Task MeController_MeUserGetOne_Tests() {
        var replacementClient = this.Fixture.CreateReplacementClient(@"unittest\otto");
        var user = await replacementClient.MeUserGetOneAsync();        
        Assert.NotNull(user);
        Assert.Equal(@"unittest\otto", user.UserName);
    }

    [Fact]
    public async Task MeController_MeProjectGetAll_Tests() {
        var replacementClient = this.Fixture.CreateReplacementClient(@"unittest\otto");
        var lstProjects = await replacementClient.MeProjectGetAllAsync();
        Assert.NotNull(lstProjects);
        Assert.True(lstProjects.Count > 0);
    }
}
