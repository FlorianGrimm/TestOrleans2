namespace Replacement.Repository.Service;

public interface ISqlAccessFactory
    : ITrackingAccessConnectionFactory {
    DBContextOption Options { get; set; }

    Task<ISqlAccess> CreateDataAccessAsync(
        CancellationToken cancellationToken = default(CancellationToken)
        );
}

[Brimborium.Registrator.Singleton(ServiceType = typeof(ISqlAccessFactory))]
public partial class SqlAccessFactory
    : ISqlAccessFactory {
    private DBContextOption _Options;

    public SqlAccessFactory(
        IOptions<DBContextOption> options
        ) {
        this._Options = options.Value;
    }

    public DBContextOption Options { get => this._Options; set => this._Options = value; }

    TrackingSqlConnectionOption ITrackingAccessConnectionFactory.GetOptions() => this._Options;

    void ITrackingAccessConnectionFactory.SetOptions(TrackingSqlConnectionOption value) {
        this._Options = (DBContextOption)value;
    }

    public async Task<ITrackingTransConnection> BeginTrackingTransConnection(
        CancellationToken cancellationToken = default(CancellationToken)
        ) {
        return await this.CreateDataAccessAsync(cancellationToken);
    }

    public async Task<ISqlAccess> CreateDataAccessAsync(
        CancellationToken cancellationToken = default(CancellationToken)
        ) {
        var connectionString = this._Options.ConnectionString;
        if (string.IsNullOrEmpty(connectionString)) { throw new InvalidOperationException("ConnectionString is empty"); }
        var (connection, transaction) = await TrackingSqlTransConnection.OpenAsync(connectionString, cancellationToken);

        var result = new SqlAccess(connection, transaction);
        return result;
    }
}