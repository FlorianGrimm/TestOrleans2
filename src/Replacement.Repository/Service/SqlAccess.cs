namespace Replacement.Repository.Service;

public partial interface ISqlAccess
    : ITrackingTransConnection
    , IDisposable {
}

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class SqlAccess : Brimborium.SqlAccess.SqlDataAccessBase, ISqlAccess {
    public SqlAccess(SqlConnection connection, IDbTransaction? transaction)
        :base(connection, transaction){
    }
}