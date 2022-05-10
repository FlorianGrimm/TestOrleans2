#if NOConverterToAPI
#else
namespace Replacement.Contracts.API;
public static partial class ConverterToEntity {
    [return: NotNullIfNotNull("that")]
    public static OperationEntity? ToOperationEntity(this Operation? that) {
        if (that is null) {
            return default;
        } else {
            return new OperationEntity(
                OperationId: that.OperationId,
                OperationName: that.OperationName,
                EntityType: that.EntityType,
                EntityId: that.EntityId,
                UserId: that.UserId,
                CreatedAt: that.CreatedAt,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<OperationEntity> ToListOperationEntity(this IEnumerable<Operation> that) {
        var result = new List<OperationEntity>();
        foreach (var item in that) { 
            result.Add(item.ToOperationEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectEntity? ToProjectEntity(this Project? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectEntity(
                ProjectId: that.ProjectId,
                Title: that.Title,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<ProjectEntity> ToListProjectEntity(this IEnumerable<Project> that) {
        var result = new List<ProjectEntity>();
        foreach (var item in that) { 
            result.Add(item.ToProjectEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectHistoryEntity? ToProjectHistoryEntity(this ProjectHistory? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectHistoryEntity(
                OperationId: that.OperationId,
                ProjectId: that.ProjectId,
                Title: that.Title,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<ProjectHistoryEntity> ToListProjectHistoryEntity(this IEnumerable<ProjectHistory> that) {
        var result = new List<ProjectHistoryEntity>();
        foreach (var item in that) { 
            result.Add(item.ToProjectHistoryEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static RequestLogEntity? ToRequestLogEntity(this RequestLog? that) {
        if (that is null) {
            return default;
        } else {
            return new RequestLogEntity(
                RequestLogId: that.RequestLogId,
                OperationId: that.OperationId,
                ActivityId: that.ActivityId,
                OperationName: that.OperationName,
                EntityType: that.EntityType,
                EntityId: that.EntityId,
                Argument: that.Argument,
                UserId: that.UserId,
                CreatedAt: that.CreatedAt,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<RequestLogEntity> ToListRequestLogEntity(this IEnumerable<RequestLog> that) {
        var result = new List<RequestLogEntity>();
        foreach (var item in that) { 
            result.Add(item.ToRequestLogEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoEntity? ToToDoEntity(this ToDo? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoEntity(
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
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<ToDoEntity> ToListToDoEntity(this IEnumerable<ToDo> that) {
        var result = new List<ToDoEntity>();
        foreach (var item in that) { 
            result.Add(item.ToToDoEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoHistoryEntity? ToToDoHistoryEntity(this ToDoHistory? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoHistoryEntity(
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
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<ToDoHistoryEntity> ToListToDoHistoryEntity(this IEnumerable<ToDoHistory> that) {
        var result = new List<ToDoHistoryEntity>();
        foreach (var item in that) { 
            result.Add(item.ToToDoHistoryEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static UserEntity? ToUserEntity(this User? that) {
        if (that is null) {
            return default;
        } else {
            return new UserEntity(
                UserId: that.UserId,
                UserName: that.UserName,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<UserEntity> ToListUserEntity(this IEnumerable<User> that) {
        var result = new List<UserEntity>();
        foreach (var item in that) { 
            result.Add(item.ToUserEntity());
        }
        return result;
    }

    [return: NotNullIfNotNull("that")]
    public static UserHistoryEntity? ToUserHistoryEntity(this UserHistory? that) {
        if (that is null) {
            return default;
        } else {
            return new UserHistoryEntity(
                OperationId: that.OperationId,
                UserId: that.UserId,
                UserName: that.UserName,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)
                );
        }
    }

    public static List<UserHistoryEntity> ToListUserHistoryEntity(this IEnumerable<UserHistory> that) {
        var result = new List<UserHistoryEntity>();
        foreach (var item in that) { 
            result.Add(item.ToUserHistoryEntity());
        }
        return result;
    }

}
#endif
