namespace Replacement.Repository.Service;

#warning WEICHEI
#if false
public partial class SqlDataAccessFactory 
    : SqlDataAccessBaseFactory<DBContextOption, SqlAccess, object> { 
}

public class TrackingSqlAccessConnection : ITrackingConnection {
    private readonly IDataAccess _SqlAccess;

    public TrackingSqlAccessConnection(IDataAccess sqlAccess) {
        this._SqlAccess = sqlAccess;
    }

    public Task<ITrackingTransConnection> BeginTrackingTransConnection() {
        var dbTransaction = this._SqlAccess.BeginTransaction();
        var result = new TrackingSqlAccessTransConnection(
            this._SqlAccess,
            this._SqlAccess.Connected(),
            dbTransaction);
        return result;
    }
}

public class TrackingSqlAccessTransConnection
    : TrackingTransConnection {
    private IDataAccess? _DataAccess;
    private IDisposable? _Connected;
    private System.Data.IDbTransaction? _DbTransaction;

    public TrackingSqlAccessTransConnection(
        IDataAccess sqlAccess,
        IDisposable connected,
        System.Data.IDbTransaction dbTransaction
        ) {
        this._DataAccess = sqlAccess;
        this._Connected = connected;
        this._DbTransaction = dbTransaction;
    }

    public System.Data.IDbTransaction? GetDbTransaction() => this._DbTransaction;

    public IDataAccess GetDataAccess() => this._DataAccess ?? throw new ObjectDisposedException("TrackingSqlAccessTransConnection");

    public override async Task CommitAsync() {
        using (var connected = this._Connected) {
            using (var dbTransaction = this._DbTransaction) {
                if (dbTransaction is Microsoft.Data.SqlClient.SqlTransaction sqlTransaction) {
                    await sqlTransaction.CommitAsync();
                    await sqlTransaction.DisposeAsync();
                } else if (dbTransaction is not null) {
                    dbTransaction.Commit();
                }
                this._DbTransaction = null;
                this._Connected = null;
            }
        }
    }

    protected override bool Dispose(bool disposing) {
        var result = base.Dispose(disposing);
        if (result) {
            using (var dataAccess = this._DataAccess) {
                using (var connected = this._Connected) {
                    using (var dbTransaction = this._DbTransaction) {
                        this._DbTransaction = null;
                        this._Connected = null;
                        this._DataAccess = null;
                    }
                }
            }
        }
        return result;
    }
}

#endif