#if NOConverterToAPI
#else
namespace Replacement.Contracts.Entity;
public static partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static Operation? ToOperation(this OperationEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new Operation(
                OperationId: that.OperationId,
                OperationName: that.OperationName,
                EntityType: that.EntityType,
                EntityId: that.EntityId,
                UserId: that.UserId,
                CreatedAt: that.CreatedAt,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<Operation> ToListOperation(this IEnumerable<OperationEntity> that) {
        var result = new List<Operation>();
        foreach (var item in that) { 
            result.Add(item.ToOperation());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static Project? ToProject(this ProjectEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new Project(
                ProjectId: that.ProjectId,
                Title: that.Title,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<Project> ToListProject(this IEnumerable<ProjectEntity> that) {
        var result = new List<Project>();
        foreach (var item in that) { 
            result.Add(item.ToProject());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectHistory? ToProjectHistory(this ProjectHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectHistory(
                OperationId: that.OperationId,
                ProjectId: that.ProjectId,
                Title: that.Title,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<ProjectHistory> ToListProjectHistory(this IEnumerable<ProjectHistoryEntity> that) {
        var result = new List<ProjectHistory>();
        foreach (var item in that) { 
            result.Add(item.ToProjectHistory());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static RequestLog? ToRequestLog(this RequestLogEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new RequestLog(
                RequestLogId: that.RequestLogId,
                OperationId: that.OperationId,
                ActivityId: that.ActivityId,
                OperationName: that.OperationName,
                EntityType: that.EntityType,
                EntityId: that.EntityId,
                Argument: that.Argument,
                UserId: that.UserId,
                CreatedAt: that.CreatedAt,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<RequestLog> ToListRequestLog(this IEnumerable<RequestLogEntity> that) {
        var result = new List<RequestLog>();
        foreach (var item in that) { 
            result.Add(item.ToRequestLog());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDo? ToToDo(this ToDoEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDo(
                ToDoId: that.ToDoId,
                ProjectId: that.ProjectId,
                UserId: that.UserId,
                Title: that.Title,
                Done: that.Done,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<ToDo> ToListToDo(this IEnumerable<ToDoEntity> that) {
        var result = new List<ToDo>();
        foreach (var item in that) { 
            result.Add(item.ToToDo());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoHistory? ToToDoHistory(this ToDoHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoHistory(
                OperationId: that.OperationId,
                ToDoId: that.ToDoId,
                ProjectId: that.ProjectId,
                UserId: that.UserId,
                Title: that.Title,
                Done: that.Done,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<ToDoHistory> ToListToDoHistory(this IEnumerable<ToDoHistoryEntity> that) {
        var result = new List<ToDoHistory>();
        foreach (var item in that) { 
            result.Add(item.ToToDoHistory());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static User? ToUser(this UserEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new User(
                UserId: that.UserId,
                UserName: that.UserName,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<User> ToListUser(this IEnumerable<UserEntity> that) {
        var result = new List<User>();
        foreach (var item in that) { 
            result.Add(item.ToUser());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static UserHistory? ToUserHistory(this UserHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new UserHistory(
                OperationId: that.OperationId,
                UserId: that.UserId,
                UserName: that.UserName,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<UserHistory> ToListUserHistory(this IEnumerable<UserHistoryEntity> that) {
        var result = new List<UserHistory>();
        foreach (var item in that) { 
            result.Add(item.ToUserHistory());
        }
        return result;
    }

}
#endif
