namespace Brimborium.RowVersion.Entity;

public struct EntryVersion {
    private long _SerialVersion;

    private string? _RowVersion;

    public EntryVersion(long serialVersion) {
        this._SerialVersion = serialVersion;
        this._RowVersion = null;
    }

    public EntryVersion(string rowVersion) {
        this._SerialVersion = 0;
        this._RowVersion = rowVersion;
    }

    public EntryVersion(long serialVersion, string rowVersion) {
        this._SerialVersion = serialVersion;
        this._RowVersion = rowVersion;
    }

    public string GetRowVersion(ref EntryVersion that) {
        if (this._RowVersion is null) {
            var result = this._SerialVersion.ToString("x16");
            that = new EntryVersion(this._SerialVersion, result);
            return result;
        } else {
            return this._RowVersion;
        }
    }

    public long GetSerialVersion(ref EntryVersion that) {
        if (this._SerialVersion == 0 && this._RowVersion is not null) {
            if (long.TryParse(this._RowVersion, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var serialVersion)) {
                that = new EntryVersion(serialVersion, this._RowVersion);
                return serialVersion;
            } else {
                return 0;
            }
        } else { 
            return this._SerialVersion;
        }
    }

    public static implicit operator EntryVersion(long serialVersion)
        => new EntryVersion(serialVersion);

    public static implicit operator EntryVersion(string rowVersion)
        => new EntryVersion(rowVersion);


    public static implicit operator string(EntryVersion entryVersion) {
        if (entryVersion._RowVersion is null) {
            return entryVersion._SerialVersion.ToString("x16");
        } else { 
            return entryVersion._RowVersion;
        }
    }

    public static implicit operator long(EntryVersion entryVersion) {
        if (entryVersion._SerialVersion == 0 && entryVersion._RowVersion is not null) {
            if (long.TryParse(entryVersion._RowVersion, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var serialVersion)) {
                return serialVersion;
            } else {
                return 0;
            }
        } else {
            return entryVersion._SerialVersion;
        }
    }
}
