namespace Brimborium.RowVersion.Entity;

public struct DataEntitiyVersion {
    private long _EntityVersion;

    private string? _DataVersion;

    public DataEntitiyVersion(long entityVersion) {
        this._EntityVersion = entityVersion;
        this._DataVersion = null;
    }

    public DataEntitiyVersion(string dataVersion) {
        this._EntityVersion = 0;
        this._DataVersion = dataVersion;
    }

    public DataEntitiyVersion(long serialVersion, string entityVersion) {
        this._EntityVersion = serialVersion;
        this._DataVersion = entityVersion;
    }

    public string GetDataVersion(ref DataEntitiyVersion that) {
        if (this._DataVersion is null) {
            var result = this._EntityVersion.ToString("x16");
            that = new DataEntitiyVersion(this._EntityVersion, result);
            return result;
        } else {
            return this._DataVersion;
        }
    }

    public long GetEntityVersion(ref DataEntitiyVersion that) {
        if (this._EntityVersion == 0 && this._DataVersion is not null) {
            if (long.TryParse(this._DataVersion, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var serialVersion)) {
                that = new DataEntitiyVersion(serialVersion, this._DataVersion);
                return serialVersion;
            } else {
                return 0;
            }
        } else { 
            return this._EntityVersion;
        }
    }

    public static implicit operator DataEntitiyVersion(long entityVersion)
        => new DataEntitiyVersion(entityVersion);

    public static implicit operator DataEntitiyVersion(string dataVersion)
        => new DataEntitiyVersion(dataVersion);


    public static implicit operator string(DataEntitiyVersion entryVersion) {
        if (entryVersion._DataVersion is null) {
            return entryVersion._EntityVersion.ToString("x16");
        } else { 
            return entryVersion._DataVersion;
        }
    }

    public static implicit operator long(DataEntitiyVersion entryVersion) {
        if (entryVersion._EntityVersion == 0 && entryVersion._DataVersion is not null) {
            if (long.TryParse(entryVersion._DataVersion, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var serialVersion)) {
                return serialVersion;
            } else {
                return 0;
            }
        } else {
            return entryVersion._EntityVersion;
        }
    }
}
