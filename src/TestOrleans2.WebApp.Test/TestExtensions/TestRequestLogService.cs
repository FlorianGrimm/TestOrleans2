namespace TestOrleans2.TestExtensions;

[ExcludeFromCodeCoverage]
[Brimborium.Registrator.Singleton]
public class TestRequestLogService : IRequestLogService {
    private readonly ITestOutputHelper? _Output;

    public TestRequestLogService(ITestOutputHelper? output) {
        this._Output = output;
    }
    public Task InsertAsync(RequestLogEntity requestLog, bool canModifyState) {
        this._Output?.WriteLine(requestLog.ToString());
        return Task.CompletedTask;
    }
}