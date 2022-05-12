namespace TestOrleans2.Contracts.Entity;

public interface IDataEntity  : Brimborium.RowVersion.Entity.IEntityWithVersion {
    // long EntityVersion { get; init; }
}

public interface IOperationRelatedEntity : IDataEntity {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    // string DataVersion { get; init; }
}

public interface IHistoryEntity : IDataEntity {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    DateTimeOffset ValidFrom { get; init; }
    DateTimeOffset ValidTo { get; init; }
    // string DataVersion { get; init; }
}