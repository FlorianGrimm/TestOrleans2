namespace Replacement.Contracts.Entity;

public interface IProjectContext {
    List<ProjectEntity> Project { get; }
    List<ToDoEntity> ToDo { get; }
}

public record class ProjectContext(
    List<ProjectEntity> Project,
    List<ToDoEntity> ToDo
) : IProjectContext;