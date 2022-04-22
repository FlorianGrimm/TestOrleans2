namespace Replacement.Repository.Storage;

public class MyGrainStorageConfiguration {
    private readonly string _ConnectionString;

    public MyGrainStorageConfiguration(
        string connectionString
        ) {
        this._ConnectionString = connectionString;
    }
    public IDBContext CreateDBContext() {
        throw new NotImplementedException();
    }
}
