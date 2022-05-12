using Replacement.Repository.Grains;

namespace Replacement.Repository.Extensions;
public class RequestOperationExtensionsTests {
    [Fact()]
    public async Task InitializeOperation_Test() {
        var userId = Guid.NewGuid();
   

        var mockUserCollectionGrain = new Mock<IUserCollectionGrain>();
        mockUserCollectionGrain.Setup(userCollectionGrain => userCollectionGrain.GetUserByUserName(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<OperationEntity>()
            )
        ).ReturnsAsync(
            (new UserEntity(userId, "testuser", Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid(), 1L), false)
        );
        IUserCollectionGrain userCollectionGrain = mockUserCollectionGrain.Object;

        var mockUserGrain = new Mock<IUserGrain>();
        mockUserGrain.Setup(userGrain => userGrain.GetUser(It.IsAny<OperationEntity>())).ReturnsAsync(new UserEntity(userId, "testuser", Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid(), 1L));
        IUserGrain userGrain = mockUserGrain.Object;

        var mockClient = new Mock<IGrainFactory>();
        mockClient.Setup(client => client.GetGrain<IUserGrain>(It.IsAny<Guid>(), It.IsAny<string>())).Returns(userGrain);
        mockClient.Setup(client => client.GetGrain<IUserCollectionGrain>(It.IsAny<Guid>(), It.IsAny<string>())).Returns(userCollectionGrain);

        IGrainFactory client = mockClient.Object;
        bool canModifyState = false;

        var mockRequestLogService = new Mock<IRequestLogService>();
        mockRequestLogService.Setup((m) => m.InsertAsync(It.IsAny<RequestLogEntity>(), canModifyState)).Returns(() => Task.CompletedTask);
        IRequestLogService requestLogService = mockRequestLogService.Object;

        {
            RequestOperation requestOperation = new RequestOperation(
               RequestLogId: Guid.NewGuid(), OperationId: Guid.NewGuid(),
               ActivityId: "a",
               OperationName: "test",
               EntityType: "test",
               EntityId: "test",
               Argument: "test",
               UserName: "testuser",
               UserId: userId,
               CreatedAt: DateTimeOffset.Now,
               SerialVersion: 0L);
            bool createUserIfNeeded = false;

            var (operation, user) = await RequestOperationExtensions.InitializeOperation(requestOperation, canModifyState, createUserIfNeeded, client, requestLogService);

            Assert.NotNull(operation);
            Assert.Equal(requestOperation.OperationId, operation.OperationId);
            Assert.NotNull(user);
            Assert.Equal(userId, user!.UserId);
        }
        {
            RequestOperation requestOperation = new RequestOperation(
               RequestLogId: Guid.NewGuid(), OperationId: Guid.NewGuid(),
               ActivityId: "a",
               OperationName: "test",
               EntityType: "test",
               EntityId: "test",
               Argument: "test",
               UserName: "testuser",
               UserId: null,
               CreatedAt: DateTimeOffset.Now,
               SerialVersion: 0L);
            bool createUserIfNeeded = false;

            var (operation, user) = await RequestOperationExtensions.InitializeOperation(requestOperation, canModifyState, createUserIfNeeded, client, requestLogService);

            Assert.NotNull(operation);
            Assert.Equal(requestOperation.OperationId, operation.OperationId);
            Assert.NotNull(user);
            Assert.Equal(userId, user!.UserId);
        }
        {
            RequestOperation requestOperation = new RequestOperation(
               RequestLogId: Guid.NewGuid(), OperationId: Guid.NewGuid(),
               ActivityId: "a",
               OperationName: "test",
               EntityType: "test",
               EntityId: "test",
               Argument: "test",
               UserName: null,
               UserId: null,
               CreatedAt: DateTimeOffset.Now,
               SerialVersion: 0L);
            bool createUserIfNeeded = false;

            var (operation, user) = await RequestOperationExtensions.InitializeOperation(requestOperation, canModifyState, createUserIfNeeded, client, requestLogService);

            Assert.NotNull(operation);
            Assert.Equal(requestOperation.OperationId, operation.OperationId);
            Assert.Null(user);
        }
        //var a = mockClient.Invocations;
        //var b = mockRequestLogService.Invocations;
    }
}
