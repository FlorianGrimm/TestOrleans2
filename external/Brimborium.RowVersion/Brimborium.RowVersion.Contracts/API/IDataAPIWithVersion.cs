namespace Brimborium.RowVersion.API;

public interface IDataAPIWithVersion {
    string DataVersion { get; }
}

[ExcludeFromCodeCoverage]
public record class DataAPIWithVersionRecord(
    string DataVersion
    ): IDataAPIWithVersion;

[ExcludeFromCodeCoverage]
public class DataAPIWithVersionClass : IDataAPIWithVersion {
    public DataAPIWithVersionClass() {
        this.DataVersion = string.Empty;
    }

    public string DataVersion { get; set; }
}
