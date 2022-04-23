﻿namespace Replacement.Repository.Service;

public partial interface ISqlAccess : IDisposable, ISqlAccessBase {
}

public partial class SqlAccess : Brimborium.SqlAccess.SqlAccessBase, ISqlAccess {
    public SqlAccess(string connectionString) : base(connectionString) {
    }
}

public class TrackingSqlAccessConnection : TrackingConnection {
    private readonly ISqlAccess _SqlAccess;

    public TrackingSqlAccessConnection(ISqlAccess sqlAccess) {
        this._SqlAccess = sqlAccess;
    }

    public override TrackingTransConnection BeginTransaction() {
#warning HERE
        using (this._SqlAccess.Connected()) {
            var dbTransaction = this._SqlAccess.BeginTransaction();
            var result = new TrackingSqlAccessTransConnection(this._SqlAccess, dbTransaction);
            return result;
        }
    }
}
public class TrackingSqlAccessTransConnection
    : TrackingTransConnection {
    private readonly ISqlAccess _SqlAccess;
    private System.Data.IDbTransaction? _DbTransaction;

    public TrackingSqlAccessTransConnection(ISqlAccess sqlAccess, System.Data.IDbTransaction dbTransaction) {
        this._SqlAccess = sqlAccess;
        this._DbTransaction = dbTransaction;
    }
    public System.Data.IDbTransaction? GetDbTransaction() => this._DbTransaction;
    public ISqlAccess GetSqlAccess() => this._SqlAccess;
    public override async Task CommitAsync() {
        using (var dbTransaction = this._DbTransaction) {
            this._DbTransaction = null;
            if (dbTransaction is Microsoft.Data.SqlClient.SqlTransaction sqlTransaction) {
                await sqlTransaction.CommitAsync();
                await sqlTransaction.DisposeAsync();
            } else if (dbTransaction is not null) {
                dbTransaction.Commit();
            }
        }
    }
}