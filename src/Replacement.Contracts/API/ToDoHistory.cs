namespace Replacement.Contracts.API;
public record class ToDoHistoryAPI(
    Guid OperationId,
    Guid ToDoId,
    Guid? ProjectId,
    Guid? UserId,
    string Title,
    bool Done,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    DateTimeOffset ValidFrom,
    DateTimeOffset ValidTo,
    long SerialVersion
) : IDataHistory;