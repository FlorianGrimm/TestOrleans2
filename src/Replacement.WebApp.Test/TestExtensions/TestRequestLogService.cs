namespace Replacement.TestExtensions;
[Brimborium.Registrator.Singleton]
public class TestRequestLogService : IRequestLogService {
    public Task InsertAsync(RequestLog requestLog, bool canModifyState) {
        return Task.CompletedTask;
    }
}
