namespace Brimborium.RowVersion.Entity;

public class DataAPIWithVersionClass
    : IEntityWithDataEntityVersion {
    private DataEntitiyVersion _EntryVersion;

    public DataAPIWithVersionClass() {
    }

    public long EntityVersion {
        get => this._EntryVersion.GetEntityVersion(ref this._EntryVersion);
        set => this._EntryVersion = new DataEntitiyVersion(value);
    }


    public string DataVersion {
        get => this.DataEntityVersion.GetDataVersion(ref this._EntryVersion);
        set => this._EntryVersion = new DataEntitiyVersion(value);
    }

    [JsonIgnore]
    public DataEntitiyVersion DataEntityVersion { get => this._EntryVersion; set => this._EntryVersion = value; }
}
