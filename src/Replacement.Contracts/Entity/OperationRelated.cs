namespace Replacement.Contracts.Entity;

public record class OperationRelatedEntity(
    OperationEntity Operation,
    ProjectEntity[] Projects,
    ToDoEntity[] ToDos,
    RequestLogEntity[] RequestLogS
);

