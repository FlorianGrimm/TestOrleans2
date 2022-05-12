namespace Replacement.Contracts.API;

public record class OperationRelated(
    Operation Operation,
    Project[] Projects,
    ToDo[] ToDos,
    RequestLog[] RequestLogs
);
