using System.Threading.Channels;

namespace Replacement.Repository.Service;

public interface IRequestLogService {
    Task InsertAsync(RequestLog requestLog, bool canModifyState);
}
public interface IRequestLogServiceBulk : IRequestLogService {
    //Task InsertAsync(RequestLog requestLog, bool canModifyState);
    Task ExecuteAsync(CancellationToken stoppingToken);
    Task FlushAsync(CancellationToken stoppingToken);
}

public class RequestLogService : IRequestLogService {
    private readonly IRequestLogServiceBulk _RequestLogServiceBulk;
    public RequestLogService(
        IRequestLogServiceBulk requestLogServiceBulk
        ) {
        this._RequestLogServiceBulk = requestLogServiceBulk;
    }

    public Task InsertAsync(RequestLog requestLog, bool canModifyState) {
        return this._RequestLogServiceBulk.InsertAsync(requestLog, canModifyState);
    }
}

public class RequestLogServiceOptions {
    public RequestLogServiceOptions() {
        this.Pause = TimeSpan.FromSeconds(5);
    }

    public bool AllLogs { get; set; }
    public TimeSpan Pause { get; set; }
}

public class RequestLogServiceBulk : IRequestLogServiceBulk {
    private readonly RequestLogServiceOptions _Options;
    private readonly Channel<RequestLog> _Channel;
    private readonly ISqlAccessFactory _SqlAccessFactory;
    private volatile Task _ChainingTask;

    public RequestLogServiceBulk(
        IOptions<RequestLogServiceOptions> options,
        ISqlAccessFactory sqlAccessFactory
        ) {
        this._Options = options.Value;
        this._SqlAccessFactory = sqlAccessFactory;
        this._Channel = System.Threading.Channels.Channel.CreateUnbounded<RequestLog>(new System.Threading.Channels.UnboundedChannelOptions() {
            SingleReader = true
        });
        this._ChainingTask = Task.CompletedTask;
    }

    public async Task InsertAsync(RequestLog requestLog, bool canModifyState) {
        if (canModifyState || this._Options.AllLogs) {
            await this._Channel.Writer.WriteAsync(requestLog);
        }
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken) {
        var reader = this._Channel.Reader;
        while (!stoppingToken.IsCancellationRequested) {
            if (await this.Flush1Async(reader, stoppingToken)) {
                try {
                    await Task.Delay(this._Options.Pause, stoppingToken);
                } catch (TaskCanceledException) {
                    await this.Flush1Async(reader, CancellationToken.None);
                    break;
                }
            }
            try {
                await reader.WaitToReadAsync(stoppingToken);
            } catch (OperationCanceledException) {
                await this.Flush1Async(reader, CancellationToken.None);
                break;
            }
        }
    }

    public async Task FlushAsync(CancellationToken stoppingToken) {
        var reader = this._Channel.Reader;
        await this.Flush1Async(reader, stoppingToken);
    }

    private async Task<bool> Flush1Async(ChannelReader<RequestLog> reader, CancellationToken stoppingToken) {
        Task<bool> flushTask;
        lock (this) {
            flushTask = this._ChainingTask.ContinueWith(async t => {
                return await this.Flush2Async(reader, stoppingToken);
            }, TaskContinuationOptions.RunContinuationsAsynchronously)
                .Unwrap();
            this._ChainingTask = flushTask;
        }
        try {
            return await flushTask;
        } finally {
            lock (this) {
                if (ReferenceEquals(flushTask, this._ChainingTask)) {
                    this._ChainingTask = Task.CompletedTask;
                }
            }
        }
    }
    private async Task<bool> Flush2Async(ChannelReader<RequestLog> reader, CancellationToken stoppingToken) {
        if (reader.TryRead(out var requestLog)) {
            using (var dataAccess = await this._SqlAccessFactory.CreateDataAccessAsync(stoppingToken)) {
                await dataAccess.ExecuteRequestLogInsertAsync(requestLog);
                while (reader.TryRead(out requestLog)) {
                    await dataAccess.ExecuteRequestLogInsertAsync(requestLog);
                }
                await dataAccess.CommitAsync();
            }
            return true;
        } else {
            return false;
        }
    }
}