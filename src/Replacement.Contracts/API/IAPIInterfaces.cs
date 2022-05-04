namespace Replacement.Contracts.API;

public interface IDataAPI {
    long SerialVersion { get; init; }
}

public interface IOperationRelatedAPI : IDataAPI {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    // long SerialVersion { get; init; }
}

public interface IHistoryAPI : IDataAPI {
    Guid OperationId { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    Guid? CreatedBy { get; init; }
    DateTimeOffset ModifiedAt { get; init; }
    Guid? ModifiedBy { get; init; }
    DateTimeOffset ValidFrom { get; init; }
    DateTimeOffset ValidTo { get; init; }
    // long SerialVersion { get; init; }
}