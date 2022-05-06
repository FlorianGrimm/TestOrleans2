﻿namespace Replacement.Contracts.API;

public record class UserHistory(
    Guid OperationId,
    Guid UserId,
    string UserName,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    DateTimeOffset ValidFrom,
    DateTimeOffset ValidTo,
    long DataVersion
) : IHistoryAPI;