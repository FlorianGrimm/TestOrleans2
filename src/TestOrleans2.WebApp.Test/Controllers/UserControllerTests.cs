namespace TestOrleans2.WebApp.Controllers;

[Collection("DefaultClusterFixture")]
public class UserControllerTests : HostedTestClusterEnsureDefaultStarted {

    public UserControllerTests(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) : base(output, fixture) {

    }

    [Fact()]
    public async Task UserController_UserAllAsync_UserGetOneAsync_Test() {
        var replacementClient = this.Fixture.CreateReplacementClient(@"unittest\otto");
        var act = await replacementClient.UserGetAllAsync();
        Assert.NotNull(act);

        Assert.True(act.Count > 0, "any user1st needed");
        var user1st = act.First();
        var userOne = await replacementClient.UserGetOneAsync(user1st.UserId);
        Assert.NotNull(userOne);

        Assert.Equal(userOne.UserId, user1st.UserId);
        Assert.Equal(userOne.UserName, user1st.UserName);
        /*
        dotnet test --filter "FullyQualifiedName~UserController_UserAllAsync_Test" 
        dotnet test --filter "FullyQualifiedName=TestOrleans2.WebApp.Controllers.UserControllerTests.UserController_UserAllAsync_Test" --diag "test.log"
        */
    }

    [Fact()]
    public async Task UserController_CUD_Test() {
        const string UserName1 = @"unittest\UserController_CUD_Test";
        const string UserName2 = @"unittest\UserController_CUD_Test_2";
        const string UserName3 = @"unittest\UserController_CUD_Test_3";
        const string UserName4 = @"unittest\UserController_CUD_Test_4";

        var replacementClient = this.Fixture.CreateReplacementClient(@"unittest\otto");
        var userInput = new User(Guid.Empty, UserName1, Guid.Empty, new DateTimeOffset(), null, new DateTimeOffset(), null, string.Empty);
        var userPostOne1 = await replacementClient.UserPostOneAsync(userInput);
        Assert.Equal(UserName1, userPostOne1.UserName);

        try {
            var userPostOne2 = await replacementClient.UserPostOneAsync(userPostOne1 with { UserName = UserName2 });
            Assert.Equal(UserName2, userPostOne2.UserName);

            var userPutOne3 = await replacementClient.UserPutOneAsync(userPostOne2.UserId, userPostOne2 with { UserName = UserName3 });
            Assert.Equal(UserName3, userPutOne3.UserName);

            // DataVersion = "1" does not match
            try {
                var userPostOne4 = await replacementClient.UserPostOneAsync(userPutOne3 with { UserName = UserName4, DataVersion = "1" });
                Assert.Null(userPostOne4);
            } catch {
            }

            var userGetOne5 = await replacementClient.UserGetOneAsync(userPostOne1.UserId);
            Assert.NotNull(userGetOne5);
            Assert.Equal(UserName3, userGetOne5.UserName);
        } finally {
            await replacementClient.UserDeleteOneAsync(userPostOne1.UserId);
        }
    }
}
