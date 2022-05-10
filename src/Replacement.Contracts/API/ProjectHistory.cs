namespace Replacement.Contracts.API;
public record class ProjectHistory(
    Guid OperationId,
    Guid ProjectId,
    string Title,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    DateTimeOffset ValidFrom,
    DateTimeOffset ValidTo,
    string DataVersion
) : IHistoryAPI;