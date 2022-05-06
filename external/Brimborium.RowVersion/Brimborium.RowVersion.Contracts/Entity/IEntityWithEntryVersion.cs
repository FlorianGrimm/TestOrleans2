namespace Brimborium.RowVersion.Entity;

public interface IEntityWithEntryVersion
    : IEntityWithVersion {

    // from IEntityWithVersion 
    // long SerialVersion { get; set; }

    string RowVersion { get; set; }

    EntryVersion EntryVersion { get; set; }
}