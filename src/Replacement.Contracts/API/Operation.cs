﻿namespace Replacement.Contracts.API;
public record class OperationAPI(
    Guid OperationId,
    string OperationName,
    string EntityType,
    string EntityId,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long SerialVersion
);
