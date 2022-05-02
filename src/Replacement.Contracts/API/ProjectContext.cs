namespace Replacement.Contracts.API;

public interface IProjectContext {
    List<Project> Project { get; }
    List<ToDo> ToDo { get; }
}

public record class ProjectContext(
    List<Project> Project,
    List<ToDo> ToDo
) : IProjectContext;