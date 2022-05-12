namespace TestOrleans2.Contracts.API;
public record class ToDoHistory(
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
    string DataVersion
) : IHistoryAPI;