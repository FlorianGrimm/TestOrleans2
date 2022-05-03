using Replacement.Contracts.Entity;

namespace Replacement.TestExtensions;
[Brimborium.Registrator.Singleton]
public class TestRequestLogService : IRequestLogService {
    public Task InsertAsync(RequestLogEntity requestLog, bool canModifyState) {
        return Task.CompletedTask;
    }
}
