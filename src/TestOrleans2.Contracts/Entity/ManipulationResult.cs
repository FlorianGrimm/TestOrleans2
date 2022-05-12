namespace TestOrleans2.Contracts.Entity;

public sealed record ProjectManipulationResult(
        ProjectEntity DataResult,
        OperationResult OperationResult
    ) : IDataManipulationResult<ProjectEntity>;

public sealed record UserManipulationResult(
        UserEntity DataResult,
        OperationResult OperationResult
    ) : IDataManipulationResult<UserEntity>;

public sealed record ToDoManipulationResult(
        ToDoEntity DataResult,
        OperationResult OperationResult
    ) : IDataManipulationResult<ToDoEntity>;
