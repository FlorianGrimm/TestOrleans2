namespace Replacement.Contracts.API;

public record class UserHistoryAPI(
    Guid OperationId,
    Guid UserId,
    string UserName,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    DateTimeOffset ValidFrom,
    DateTimeOffset ValidTo,
    long SerialVersion
) : IDataHistory;