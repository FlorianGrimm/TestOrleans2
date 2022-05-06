namespace Brimborium.RowVersion.API;

public interface IDataAPIWithVersion {
    string RowVersion { get; }
}

public record class DataAPIWithVersionRecord(
    string RowVersion
    );

public class DataAPIWithVersionClass {
    public DataAPIWithVersionClass() {
        this.RowVersion = string.Empty;
    }

    public string RowVersion { get; set; }
}
