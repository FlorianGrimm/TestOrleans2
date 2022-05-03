#if NOConverterToAPI
#else
namespace Replacement.Contracts.API;
public static partial class ConverterToEntity {
    [return: NotNullIfNotNull("that")]
    public static OperationEntity? ToOperationEntity(this OperationAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectEntity? ToProjectEntity(this ProjectAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static ProjectHistoryEntity? ToProjectHistoryEntity(this ProjectHistoryAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static RequestLogEntity? ToRequestLogEntity(this RequestLogAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static RequestOperationEntity? ToRequestOperationEntity(this RequestOperationAPI? that) {
        if (that is null) {
            return default;
        } else {
            return new RequestOperationEntity(
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

    [return: NotNullIfNotNull("that")]
    public static ToDoEntity? ToToDoEntity(this ToDoAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static ToDoHistoryEntity? ToToDoHistoryEntity(this ToDoHistoryAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static UserEntity? ToUserEntity(this UserAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

    [return: NotNullIfNotNull("that")]
    public static UserHistoryEntity? ToUserHistoryEntity(this UserHistoryAPI? that) {
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
                SerialVersion: that.SerialVersion
                );
        }
    }

}
#endif
