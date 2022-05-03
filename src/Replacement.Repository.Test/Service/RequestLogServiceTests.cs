using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;
public class RequestLogServiceTests {
    [Fact]
    public async Task RequestLogService_001Async() {
        var options = new RequestLogServiceOptions() {
            AllLogs = true,
            Pause = TimeSpan.FromMilliseconds(5)
        };
        var sqlAccessFactory = new TestISqlAccessFactory();
        var requestLogServiceBulk = new RequestLogServiceBulk(
            Options.Create<RequestLogServiceOptions>(options),
            sqlAccessFactory);
        var requestLogService = new RequestLogService(requestLogServiceBulk);
        var requestLogs = System.Linq.Enumerable.Range(1, 10)
            .Select(i => new RequestLog(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid().ToString(), i.ToString(), i.ToString(), i.ToString(), i.ToString(), null, DateTimeOffset.MinValue, i))
            .ToList();
        var cts = new CancellationTokenSource();
        var bulkTask = requestLogServiceBulk.ExecuteAsync(cts.Token);
        await requestLogService.InsertAsync(requestLogs[0], true);
        await requestLogService.InsertAsync(requestLogs[1], true);
        var task1 = requestLogServiceBulk.FlushAsync(CancellationToken.None);
        await requestLogService.InsertAsync(requestLogs[2], true);
        await requestLogService.InsertAsync(requestLogs[3], true);
        var task2 = requestLogServiceBulk.FlushAsync(CancellationToken.None);
        await requestLogService.InsertAsync(requestLogs[4], true);
        await requestLogService.InsertAsync(requestLogs[5], true);
        await requestLogService.InsertAsync(requestLogs[6], true);
        await requestLogService.InsertAsync(requestLogs[7], true);
        await requestLogService.InsertAsync(requestLogs[8], true);
        await task1;
        await task2;
        await requestLogServiceBulk.FlushAsync(CancellationToken.None);
        Assert.Equal(9, sqlAccessFactory.RequestLogs.Count);

        await requestLogService.InsertAsync(requestLogs[9], true);
        await Task.Delay(1);
        cts.Cancel();
        await bulkTask;
        /*
        await Assert.ThrowsAsync<TaskCanceledException>(
            async () => { await bulkTask; }
            );
        */
        Assert.Equal(10, sqlAccessFactory.RequestLogs.Count);
        for (int idx = 0; idx < sqlAccessFactory.RequestLogs.Count; idx++) {
            var a = sqlAccessFactory.RequestLogs[idx];
            Assert.Equal(idx+1, a.rl.SerialVersion);
        }
        for (int idx = 1; idx < sqlAccessFactory.RequestLogs.Count; idx++) {
            var a = sqlAccessFactory.RequestLogs[idx - 1];
            var b = sqlAccessFactory.RequestLogs[idx];
            Assert.True(a.ts < b.ts);
        }
    }

    class TestISqlAccessFactory : ISqlAccessFactory, ISqlAccess {
        public List<(TimeSpan ts, RequestLog rl)> RequestLogs { get; }
        public DateTime StartedAt { get; }
        public bool Commited { get; private set; }

        public TestISqlAccessFactory() {
            this.StartedAt = DateTime.UtcNow;
            this.RequestLogs = new List<(TimeSpan ts, RequestLog rl)>();
        }
        public DBContextOption Options { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<ISqlAccess> CreateDataAccessAsync(CancellationToken cancellationToken = default) {
            return Task.FromResult<ISqlAccess>(this);
        }

        public Task CommitAsync() {
            this.Commited = true;
            return Task.CompletedTask;
        }

        public void Dispose() {
        }

        public async Task ExecuteRequestLogInsertAsync(RequestLog args) {
            var ts = DateTime.UtcNow - this.StartedAt;
            this.RequestLogs.Add((ts, args));
            await Task.Delay(this.RequestLogs.Count);
        }

        public Task<ITrackingTransConnection> BeginTrackingTransConnection(CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<Operation> ExecuteOperationInsertAsync(Operation args) {
            throw new NotImplementedException();
        }

        public Task<Operation?> ExecuteOperationSelectPKAsync(OperationPK args) {
            throw new NotImplementedException();
        }

        public Task<List<ProjectPK>> ExecuteProjectDeletePKAsync(Project args) {
            throw new NotImplementedException();
        }

        public Task<List<Project>> ExecuteProjectSelectAllAsync() {
            throw new NotImplementedException();
        }

        public Task<ProjectSelectPKResult> ExecuteProjectSelectPKAsync(ProjectPK args) {
            throw new NotImplementedException();
        }

        public Task<ProjectManipulationResult> ExecuteProjectUpsertAsync(Project args) {
            throw new NotImplementedException();
        }

        public Task<List<ToDoPK>> ExecuteToDoDeletePKAsync(ToDo args) {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> ExecuteToDoSelectAllAsync() {
            throw new NotImplementedException();
        }

        public Task<ToDo?> ExecuteToDoSelectPKAsync(ToDoPK args) {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> ExecuteToDoSelectProjectAsync(ToDoPK args) {
            throw new NotImplementedException();
        }

        public Task<ToDoManipulationResult> ExecuteToDoUpsertAsync(ToDo args) {
            throw new NotImplementedException();
        }

        public Task<List<UserPK>> ExecuteUserDeletePKAsync(User args) {
            throw new NotImplementedException();
        }

        public Task<User?> ExecuteUserSelectByUserNameAsync(UserSelectByUserNameArg args) {
            throw new NotImplementedException();
        }

        public Task<User?> ExecuteUserSelectPKAsync(UserPK args) {
            throw new NotImplementedException();
        }

        public Task<UserManipulationResult> ExecuteUserUpsertAsync(User args) {
            throw new NotImplementedException();
        }

        public TrackingSqlConnectionOption GetOptions() {
            throw new NotImplementedException();
        }

        public void SetOptions(TrackingSqlConnectionOption value) {
            throw new NotImplementedException();
        }
    }
}
