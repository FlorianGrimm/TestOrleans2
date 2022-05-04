namespace Replacement.Contracts.Entity;

public interface IDataEntity {
    long SerialVersion { get; init; }
}

public interface IOperationRelatedEntity : IDataEntity {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    // long SerialVersion { get; init; }
}

public interface IHistoryEntity : IDataEntity {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    DateTimeOffset ValidFrom { get; init; }
    DateTimeOffset ValidTo { get; init; }
    // long SerialVersion { get; init; }
}