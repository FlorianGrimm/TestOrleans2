namespace Brimborium.RowVersion.Entity;

public struct AggregationRowVersion {
    public long RowVersion { get; set; }
    public int CountVersion { get; set; }
    public string Arguments { get; set; }
}

public sealed class AggregationRowVersionHolder {
    public AggregationRowVersionHolder() {
        this.Value = new AggregationRowVersion();
    }
    public AggregationRowVersion Value;
}
