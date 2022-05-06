namespace Brimborium.RowVersion.Entity;

public class DataAPIWithVersionClass
    : IEntityWithEntryVersion {
    private EntryVersion _EntryVersion;

    public DataAPIWithVersionClass() {
    }

    public long SerialVersion {
        get => this._EntryVersion.GetSerialVersion(ref this._EntryVersion);
        set => this._EntryVersion = new EntryVersion(value);
    }


    public string RowVersion {
        get => this.EntryVersion.GetRowVersion(ref this._EntryVersion);
        set => this._EntryVersion = new EntryVersion(value);
    }

    [JsonIgnore]
    public EntryVersion EntryVersion { get => this._EntryVersion; set => this._EntryVersion = value; }
}
