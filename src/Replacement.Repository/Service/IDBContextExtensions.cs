namespace Replacement.Repository.Service;

public static class IDBContextExtensions {
    public static Project? GetProject(this IDBContext that, ProjectPK pk) {
        that.Project.TryGetValue(pk, out var result);
        return result;
    }

    public static IEnumerable<Project> GetProjectOfUserCreatedBy(this IDBContext that, UserPK userPK) {
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

    public static IEnumerable<ToDo> GetToDoOfProject(this IDBContext that, ProjectPK projectPK) {
        return that.ToDo.Values.Where(
                toDo => toDo.GetProjectPK().Equals(projectPK)
            );
    }
}