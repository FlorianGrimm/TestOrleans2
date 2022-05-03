namespace Replacement.Contracts.Entity;

public interface IDataCommon {
    long SerialVersion { get; init; }
}

public interface IDataOperationRelated : IDataCommon {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    // long SerialVersion { get; init; }
}

public interface IDataHistory : IDataCommon {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    DateTimeOffset ValidFrom { get; init; }
    DateTimeOffset ValidTo { get; init; }
    // long SerialVersion { get; init; }
}