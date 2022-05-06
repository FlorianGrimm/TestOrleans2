namespace Brimborium.RowVersion.Entity;

public interface IEntityWithVersion {
    long SerialVersion { get; }
}
