using TestOrleans2.Contracts.Entity;

namespace TestOrleans2.Repository.Service;

public static class IDBContextExtensions {
    public static ProjectEntity? GetProject(this IDBContext that, ProjectPK pk) {
        that.Project.TryGetValue(pk, out var result);
        return result;
    }

    public static IEnumerable<ProjectEntity> GetProjectOfUserCreatedBy(this IDBContext that, UserPK userPK) {
        return that.Project.Values.Where(
                project => userPK.Equals(project.GetCreatedByUserPK())
            );
    }

#if false
    public static IEnumerable<ToDo> GetToDoOfUserCreatedBy(this IDBContext that, ProjectPK projectPK, UserPK userPK) {
        return that.ToDo.Values.Where(
                todo => userPK.Equals(todo.GetCreatedByUserPK())
                        && projectPK.Equals(todo.GetProjectPK())
            );
    }
#endif

    public static IEnumerable<ToDoEntity> GetToDoOfProject(this IDBContext that, ProjectPK projectPK) {
        return that.ToDo.Values.Where(
                toDo => toDo.GetProjectPK().Equals(projectPK)
            );
    }
}