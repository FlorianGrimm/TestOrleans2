#if true
#nullable enable


using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service {
    partial class SqlAccess {
        public async Task<OperationEntity> ExecuteOperationInsertAsync(OperationEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleAsync<OperationEntity>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected OperationEntity ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new OperationEntity(
                @OperationId: this.ReadGuid(reader, 0),
                @OperationName: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @CreatedAt: this.ReadDateTimeOffset(reader, 4),
                @UserId: this.ReadGuidQ(reader, 5),
                @SerialVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<OperationEntity?> ExecuteOperationSelectPKAsync(OperationPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                return await this.CommandQuerySingleOrDefaultAsync<OperationEntity>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected OperationEntity ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new OperationEntity(
                @OperationId: this.ReadGuid(reader, 0),
                @OperationName: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @CreatedAt: this.ReadDateTimeOffset(reader, 4),
                @UserId: this.ReadGuidQ(reader, 5),
                @SerialVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<List<ProjectPK>> ExecuteProjectDeletePKAsync(ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ProjectPK(
                @ProjectId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<ProjectEntity>> ExecuteProjectSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<ProjectEntity>(cmd, ReadRecordProjectSelectAll);
            }
        } 

        protected ProjectEntity ReadRecordProjectSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<ProjectSelectPKResult> ExecuteProjectSelectPKAsync(ProjectPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                System.Collections.Generic.List<ProjectEntity> result_Projects = new System.Collections.Generic.List<ProjectEntity>();
                System.Collections.Generic.List<ToDoEntity> result_ToDos = new System.Collections.Generic.List<ToDoEntity>();
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_Projects = await this.CommandReadQueryAsync<ProjectEntity>(reader, ReadRecordProjectSelectPK_0);
                    }
                    if (idx == 1) {
                        result_ToDos = await this.CommandReadQueryAsync<ToDoEntity>(reader, ReadRecordProjectSelectPK_1);
                    }
                } , 2);
                var result = new ProjectSelectPKResult(
                    Projects: result_Projects,
                    ToDos: result_ToDos
                );
                return result;
            }
        } 

        protected ProjectEntity ReadRecordProjectSelectPK_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected ToDoEntity ReadRecordProjectSelectPK_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1),
                @UserId: this.ReadGuid(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuid(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @CreatedBy: this.ReadGuidQ(reader, 7),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 8),
                @ModifiedBy: this.ReadGuidQ(reader, 9),
                @SerialVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<ProjectManipulationResult> ExecuteProjectUpsertAsync(ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                ProjectEntity result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<ProjectEntity>(reader, ReadRecordProjectUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordProjectUpsert_1);
                    }
                } , 2);
                var result = new ProjectManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected ProjectEntity ReadRecordProjectUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected OperationResult ReadRecordProjectUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task ExecuteRequestLogInsertAsync(RequestLogEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[RequestLogInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@RequestLogId", args.RequestLogId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@ActivityId", SqlDbType.VarChar, 200, args.ActivityId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterString(cmd, "@Argument", SqlDbType.NVarChar, -1, args.Argument);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                await this.CommandExecuteNonQueryAsync(cmd);
                return;
            }
        } 

        public async Task<List<ToDoPK>> ExecuteToDoDeletePKAsync(ToDoEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<ToDoPK>(cmd, ReadRecordToDoDeletePK);
            }
        } 

        protected ToDoPK ReadRecordToDoDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoPK(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1)
            );
            return result;
        } 

        public async Task<List<ToDoEntity>> ExecuteToDoSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<ToDoEntity>(cmd, ReadRecordToDoSelectAll);
            }
        } 

        protected ToDoEntity ReadRecordToDoSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1),
                @UserId: this.ReadGuid(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuid(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @CreatedBy: this.ReadGuidQ(reader, 7),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 8),
                @ModifiedBy: this.ReadGuidQ(reader, 9),
                @SerialVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<ToDoEntity?> ExecuteToDoSelectPKAsync(ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQuerySingleOrDefaultAsync<ToDoEntity>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected ToDoEntity ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1),
                @UserId: this.ReadGuid(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuid(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @CreatedBy: this.ReadGuidQ(reader, 7),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 8),
                @ModifiedBy: this.ReadGuidQ(reader, 9),
                @SerialVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<List<ToDoEntity>> ExecuteToDoSelectProjectAsync(ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectProject]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQueryAsync<ToDoEntity>(cmd, ReadRecordToDoSelectProject);
            }
        } 

        protected ToDoEntity ReadRecordToDoSelectProject(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1),
                @UserId: this.ReadGuid(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuid(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @CreatedBy: this.ReadGuidQ(reader, 7),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 8),
                @ModifiedBy: this.ReadGuidQ(reader, 9),
                @SerialVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<ToDoManipulationResult> ExecuteToDoUpsertAsync(ToDoEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterBoolean(cmd, "@Done", args.Done);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                ToDoEntity result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<ToDoEntity>(reader, ReadRecordToDoUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordToDoUpsert_1);
                    }
                } , 2);
                var result = new ToDoManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected ToDoEntity ReadRecordToDoUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new ToDoEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1),
                @UserId: this.ReadGuid(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuid(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @CreatedBy: this.ReadGuidQ(reader, 7),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 8),
                @ModifiedBy: this.ReadGuidQ(reader, 9),
                @SerialVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        protected OperationResult ReadRecordToDoUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<UserPK>> ExecuteUserDeletePKAsync(UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new UserPK(
                @UserId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<UserEntity?> ExecuteUserSelectByUserNameAsync(UserSelectByUserNameArg args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<UserEntity>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected UserEntity ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<UserEntity?> ExecuteUserSelectPKAsync(UserPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleOrDefaultAsync<UserEntity>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected UserEntity ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<UserManipulationResult> ExecuteUserUpsertAsync(UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                UserEntity result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<UserEntity>(reader, ReadRecordUserUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordUserUpsert_1);
                    }
                } , 2);
                var result = new UserManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected UserEntity ReadRecordUserUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected OperationResult ReadRecordUserUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

    }
    partial interface ISqlAccess {
        Task<OperationEntity> ExecuteOperationInsertAsync(OperationEntity args);
        Task<OperationEntity?> ExecuteOperationSelectPKAsync(OperationPK args);
        Task<List<ProjectPK>> ExecuteProjectDeletePKAsync(ProjectEntity args);
        Task<List<ProjectEntity>> ExecuteProjectSelectAllAsync();
        Task<ProjectSelectPKResult> ExecuteProjectSelectPKAsync(ProjectPK args);
        Task<ProjectManipulationResult> ExecuteProjectUpsertAsync(ProjectEntity args);
        Task ExecuteRequestLogInsertAsync(RequestLogEntity args);
        Task<List<ToDoPK>> ExecuteToDoDeletePKAsync(ToDoEntity args);
        Task<List<ToDoEntity>> ExecuteToDoSelectAllAsync();
        Task<ToDoEntity?> ExecuteToDoSelectPKAsync(ToDoPK args);
        Task<List<ToDoEntity>> ExecuteToDoSelectProjectAsync(ToDoPK args);
        Task<ToDoManipulationResult> ExecuteToDoUpsertAsync(ToDoEntity args);
        Task<List<UserPK>> ExecuteUserDeletePKAsync(UserEntity args);
        Task<UserEntity?> ExecuteUserSelectByUserNameAsync(UserSelectByUserNameArg args);
        Task<UserEntity?> ExecuteUserSelectPKAsync(UserPK args);
        Task<UserManipulationResult> ExecuteUserUpsertAsync(UserEntity args);
    }
}

#endif
