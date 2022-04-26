namespace Replacement.Repository.Service;

public partial interface ISqlAccess
    : ITrackingTransConnection
    , IDisposable {
}

public partial class SqlAccess : Brimborium.SqlAccess.SqlDataAccessBase, ISqlAccess {
    public SqlAccess(SqlConnection connection, IDbTransaction? transaction)
        :base(connection, transaction){
    }

    //public SqlAccess(string connectionString) 
    //    : base(connectionString) {
    //}

    //public TrackingTransConnection BeginTransaction() {
    //    var dbTransaction = this._SqlAccess.BeginTransaction();
    //    var result = new TrackingSqlAccessTransConnection(
    //        this._SqlAccess,
    //        this._SqlAccess.Connected(),
    //        dbTransaction);
    //    return result;
    //}
}