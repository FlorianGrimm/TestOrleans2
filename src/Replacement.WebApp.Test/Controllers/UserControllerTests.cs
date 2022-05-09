namespace Replacement.WebApp.Controllers;
[Collection("DefaultClusterFixture")]
public class UserControllerTests : HostedTestClusterEnsureDefaultStarted {
    public UserControllerTests(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) : base(output, fixture) {

    }

    [Fact()]
    public async Task UserController_UserAllAsync_Test() {
        //new UserController()
        var client = this.Fixture.CreateWebClient();
        var baseAddress = this.Fixture.WebTestServer.BaseAddress!;
        var replacementClient = new Replacement.Client.ReplacementClient(baseAddress.ToString(), client);
        var act = await replacementClient.UserAllAsync();
        Assert.NotNull(act);

        /*
        dotnet test --filter "FullyQualifiedName~UserController_UserAllAsync_Test" 
        dotnet test --filter "FullyQualifiedName=Replacement.WebApp.Controllers.UserControllerTests.UserController_UserAllAsync_Test" --diag "test.log"
        */
    }

    [Fact()]
    public void Get_Test1() {

    }

    [Fact()]
    public void Post_Test() {

    }

    [Fact()]
    public void Put_Test() {

    }

    [Fact()]
    public void Delete_Test() {

    }
}
