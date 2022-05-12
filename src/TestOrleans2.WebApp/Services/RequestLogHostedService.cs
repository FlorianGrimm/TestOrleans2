namespace TestOrleans2.WebApp.Services;

public class RequestLogHostedService
    : BackgroundService {
    private readonly IRequestLogServiceBulk _RequestLogServiceBulk;

    public RequestLogHostedService(
        IRequestLogServiceBulk requestLogServiceBulk
        ) {
        this._RequestLogServiceBulk = requestLogServiceBulk;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        await this._RequestLogServiceBulk.ExecuteAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken) {
        await this._RequestLogServiceBulk.FlushAsync(CancellationToken.None);
        await base.StopAsync(cancellationToken);
    }
}

