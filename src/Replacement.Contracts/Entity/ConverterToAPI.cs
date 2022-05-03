#if NOConverterToAPI
#else
namespace Replacement.Contracts.Entity;
public static partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static OperationAPI? ToOperationAPI(this OperationEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new OperationAPI(
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

    public static List<OperationAPI> ToListOperationAPI(this IEnumerable<OperationEntity> that) {
        var result = new List<OperationAPI>();
        foreach (var item in that) { 
            result.Add(item.ToOperationAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectAPI? ToProjectAPI(this ProjectEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectAPI(
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

    public static List<ProjectAPI> ToListProjectAPI(this IEnumerable<ProjectEntity> that) {
        var result = new List<ProjectAPI>();
        foreach (var item in that) { 
            result.Add(item.ToProjectAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectHistoryAPI? ToProjectHistoryAPI(this ProjectHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectHistoryAPI(
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

    public static List<ProjectHistoryAPI> ToListProjectHistoryAPI(this IEnumerable<ProjectHistoryEntity> that) {
        var result = new List<ProjectHistoryAPI>();
        foreach (var item in that) { 
            result.Add(item.ToProjectHistoryAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static RequestLogAPI? ToRequestLogAPI(this RequestLogEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new RequestLogAPI(
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

    public static List<RequestLogAPI> ToListRequestLogAPI(this IEnumerable<RequestLogEntity> that) {
        var result = new List<RequestLogAPI>();
        foreach (var item in that) { 
            result.Add(item.ToRequestLogAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static RequestOperationAPI? ToRequestOperationAPI(this RequestOperationEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new RequestOperationAPI(
                RequestLogId: that.RequestLogId,
                OperationId: that.OperationId,
                ActivityId: that.ActivityId,
                OperationName: that.OperationName,
                EntityType: that.EntityType,
                EntityId: that.EntityId,
                Argument: that.Argument,
                UserName: that.UserName,
                UserId: that.UserId,
                CreatedAt: that.CreatedAt,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<RequestOperationAPI> ToListRequestOperationAPI(this IEnumerable<RequestOperationEntity> that) {
        var result = new List<RequestOperationAPI>();
        foreach (var item in that) { 
            result.Add(item.ToRequestOperationAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoAPI? ToToDoAPI(this ToDoEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoAPI(
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

    public static List<ToDoAPI> ToListToDoAPI(this IEnumerable<ToDoEntity> that) {
        var result = new List<ToDoAPI>();
        foreach (var item in that) { 
            result.Add(item.ToToDoAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoHistoryAPI? ToToDoHistoryAPI(this ToDoHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoHistoryAPI(
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

    public static List<ToDoHistoryAPI> ToListToDoHistoryAPI(this IEnumerable<ToDoHistoryEntity> that) {
        var result = new List<ToDoHistoryAPI>();
        foreach (var item in that) { 
            result.Add(item.ToToDoHistoryAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static UserAPI? ToUserAPI(this UserEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new UserAPI(
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

    public static List<UserAPI> ToListUserAPI(this IEnumerable<UserEntity> that) {
        var result = new List<UserAPI>();
        foreach (var item in that) { 
            result.Add(item.ToUserAPI());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static UserHistoryAPI? ToUserHistoryAPI(this UserHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new UserHistoryAPI(
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

    public static List<UserHistoryAPI> ToListUserHistoryAPI(this IEnumerable<UserHistoryEntity> that) {
        var result = new List<UserHistoryAPI>();
        foreach (var item in that) { 
            result.Add(item.ToUserHistoryAPI());
        }
        return result;
    }

}
#endif
