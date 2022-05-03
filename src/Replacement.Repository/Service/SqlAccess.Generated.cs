#if true
#nullable enable


using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service {
    partial class SqlAccess {
        public async Task<Operation> ExecuteOperationInsertAsync(Operation args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleAsync<Replacement.Contracts.Entity.Operation>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected Operation ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.Operation(
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

        public async Task<Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.Entity.OperationPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.Operation>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected Operation ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.Operation(
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

        public async Task<List<Replacement.Contracts.Entity.ProjectPK>> ExecuteProjectDeletePKAsync(Project args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected Replacement.Contracts.Entity.ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ProjectPK(
                @ProjectId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<Project>> ExecuteProjectSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.Project>(cmd, ReadRecordProjectSelectAll);
            }
        } 

        protected Project ReadRecordProjectSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.Project(
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

        public async Task<ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.Entity.ProjectPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                System.Collections.Generic.List<Replacement.Contracts.Entity.Project> result_Projects = new System.Collections.Generic.List<Replacement.Contracts.Entity.Project>();
                System.Collections.Generic.List<Replacement.Contracts.Entity.ToDo> result_ToDos = new System.Collections.Generic.List<Replacement.Contracts.Entity.ToDo>();
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_Projects = await this.CommandReadQueryAsync<Replacement.Contracts.Entity.Project>(reader, ReadRecordProjectSelectPK_0);
                    }
                    if (idx == 1) {
                        result_ToDos = await this.CommandReadQueryAsync<Replacement.Contracts.Entity.ToDo>(reader, ReadRecordProjectSelectPK_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ProjectSelectPKResult(
                    Projects: result_Projects,
                    ToDos: result_ToDos
                );
                return result;
            }
        } 

        protected Project ReadRecordProjectSelectPK_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.Project(
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

        protected ToDo ReadRecordProjectSelectPK_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDo(
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

        public async Task<ProjectManipulationResult> ExecuteProjectUpsertAsync(Project args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                Project result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Project>(reader, ReadRecordProjectUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordProjectUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ProjectManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Project ReadRecordProjectUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.Project(
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
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task ExecuteRequestLogInsertAsync(RequestLog args)  {
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

        public async Task<List<Replacement.Contracts.Entity.ToDoPK>> ExecuteToDoDeletePKAsync(ToDo args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ToDoPK>(cmd, ReadRecordToDoDeletePK);
            }
        } 

        protected Replacement.Contracts.Entity.ToDoPK ReadRecordToDoDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoPK(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1)
            );
            return result;
        } 

        public async Task<List<ToDo>> ExecuteToDoSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ToDo>(cmd, ReadRecordToDoSelectAll);
            }
        } 

        protected ToDo ReadRecordToDoSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDo(
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

        public async Task<ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.Entity.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.ToDo>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected ToDo ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDo(
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

        public async Task<List<ToDo>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.Entity.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectProject]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ToDo>(cmd, ReadRecordToDoSelectProject);
            }
        } 

        protected ToDo ReadRecordToDoSelectProject(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDo(
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

        public async Task<ToDoManipulationResult> ExecuteToDoUpsertAsync(ToDo args)  {
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
                ToDo result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<ToDo>(reader, ReadRecordToDoUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordToDoUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ToDoManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected ToDo ReadRecordToDoUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDo(
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
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<Replacement.Contracts.Entity.UserPK>> ExecuteUserDeletePKAsync(User args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected Replacement.Contracts.Entity.UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.UserPK(
                @UserId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<User?> ExecuteUserSelectByUserNameAsync(UserSelectByUserNameArg args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.User>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected User ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.User(
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

        public async Task<User?> ExecuteUserSelectPKAsync(Replacement.Contracts.Entity.UserPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.User>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected User ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.User(
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

        public async Task<UserManipulationResult> ExecuteUserUpsertAsync(User args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                User result_DataResult = default!;
                OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<User>(reader, ReadRecordUserUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<OperationResult>(reader, ReadRecordUserUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.UserManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected User ReadRecordUserUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.User(
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
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

    }
    partial interface ISqlAccess {
        Task<Operation> ExecuteOperationInsertAsync(Operation args);
        Task<Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.Entity.OperationPK args);
        Task<List<Replacement.Contracts.Entity.ProjectPK>> ExecuteProjectDeletePKAsync(Project args);
        Task<List<Project>> ExecuteProjectSelectAllAsync();
        Task<ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.Entity.ProjectPK args);
        Task<ProjectManipulationResult> ExecuteProjectUpsertAsync(Project args);
        Task ExecuteRequestLogInsertAsync(RequestLog args);
        Task<List<Replacement.Contracts.Entity.ToDoPK>> ExecuteToDoDeletePKAsync(ToDo args);
        Task<List<ToDo>> ExecuteToDoSelectAllAsync();
        Task<ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.Entity.ToDoPK args);
        Task<List<ToDo>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.Entity.ToDoPK args);
        Task<ToDoManipulationResult> ExecuteToDoUpsertAsync(ToDo args);
        Task<List<Replacement.Contracts.Entity.UserPK>> ExecuteUserDeletePKAsync(User args);
        Task<User?> ExecuteUserSelectByUserNameAsync(UserSelectByUserNameArg args);
        Task<User?> ExecuteUserSelectPKAsync(Replacement.Contracts.Entity.UserPK args);
        Task<UserManipulationResult> ExecuteUserUpsertAsync(User args);
    }
}

#endif
