#if true
#nullable enable


namespace Replacement.Repository.Service {
    partial class SqlAccess {
        public async Task<Replacement.Contracts.API.Operation> ExecuteOperationInsertAsync(Replacement.Contracts.API.Operation args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 20, args.Title);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.NVarChar, 100, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterString(cmd, "@Data", SqlDbType.NVarChar, -1, args.Data);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleAsync<Replacement.Contracts.API.Operation>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected Replacement.Contracts.API.Operation ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Operation(
                @OperationId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @Data: this.ReadString(reader, 4),
                @CreatedAt: this.ReadDateTimeOffset(reader, 5),
                @UserId: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.Operation>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected Replacement.Contracts.API.Operation ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Operation(
                @OperationId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @Data: this.ReadString(reader, 4),
                @CreatedAt: this.ReadDateTimeOffset(reader, 5),
                @UserId: this.ReadGuidQ(reader, 6),
                @SerialVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.API.Project args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected Replacement.Contracts.API.ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ProjectPK(
                @ProjectId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.Project>> ExecuteProjectSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.API.Project>(cmd, ReadRecordProjectSelectAll);
            }
        } 

        protected Replacement.Contracts.API.Project ReadRecordProjectSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Project(
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

        public async Task<Replacement.Contracts.API.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                System.Collections.Generic.List<Replacement.Contracts.API.Project> result_Projects = new System.Collections.Generic.List<Replacement.Contracts.API.Project>();
                System.Collections.Generic.List<Replacement.Contracts.API.ToDo> result_ToDos = new System.Collections.Generic.List<Replacement.Contracts.API.ToDo>();
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_Projects = await this.CommandReadQueryAsync<Replacement.Contracts.API.Project>(reader, ReadRecordProjectSelectPK_0);
                    }
                    if (idx == 1) {
                        result_ToDos = await this.CommandReadQueryAsync<Replacement.Contracts.API.ToDo>(reader, ReadRecordProjectSelectPK_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.API.ProjectSelectPKResult(
                    Projects: result_Projects,
                    ToDos: result_ToDos
                );
                return result;
            }
        } 

        protected Replacement.Contracts.API.Project ReadRecordProjectSelectPK_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Project(
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

        protected Replacement.Contracts.API.ToDo ReadRecordProjectSelectPK_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
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

        public async Task<Replacement.Contracts.API.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.API.Project args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                Replacement.Contracts.API.Project result_DataResult = default!;
                Replacement.Contracts.API.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.Project>(reader, ReadRecordProjectUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.OperationResult>(reader, ReadRecordProjectUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.API.ProjectManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.API.Project ReadRecordProjectUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Project(
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

        protected Replacement.Contracts.API.OperationResult ReadRecordProjectUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.OperationResult(
                @resultValue: (Replacement.Contracts.API.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.API.ToDo args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ToDoPK>(cmd, ReadRecordToDoDeletePK);
            }
        } 

        protected Replacement.Contracts.API.ToDoPK ReadRecordToDoDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDoPK(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.ToDo>> ExecuteToDoSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.API.ToDo>(cmd, ReadRecordToDoSelectAll);
            }
        } 

        protected Replacement.Contracts.API.ToDo ReadRecordToDoSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
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

        public async Task<Replacement.Contracts.API.ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.ToDo>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected Replacement.Contracts.API.ToDo ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
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

        public async Task<List<Replacement.Contracts.API.ToDo>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectProject]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ToDo>(cmd, ReadRecordToDoSelectProject);
            }
        } 

        protected Replacement.Contracts.API.ToDo ReadRecordToDoSelectProject(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
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

        public async Task<Replacement.Contracts.API.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.API.ToDo args)  {
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
                Replacement.Contracts.API.ToDo result_DataResult = default!;
                Replacement.Contracts.API.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.ToDo>(reader, ReadRecordToDoUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.OperationResult>(reader, ReadRecordToDoUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.API.ToDoManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.API.ToDo ReadRecordToDoUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
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

        protected Replacement.Contracts.API.OperationResult ReadRecordToDoUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.OperationResult(
                @resultValue: (Replacement.Contracts.API.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.API.User args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected Replacement.Contracts.API.UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.UserPK(
                @UserId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.User?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.API.UserSelectByUserNameArg args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.User>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected Replacement.Contracts.API.User ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.User(
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

        public async Task<Replacement.Contracts.API.User?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.User>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected Replacement.Contracts.API.User ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.User(
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

        public async Task<Replacement.Contracts.API.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.API.User args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                Replacement.Contracts.API.User result_DataResult = default!;
                Replacement.Contracts.API.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.User>(reader, ReadRecordUserUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.API.OperationResult>(reader, ReadRecordUserUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.API.UserManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.API.User ReadRecordUserUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.User(
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

        protected Replacement.Contracts.API.OperationResult ReadRecordUserUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.OperationResult(
                @resultValue: (Replacement.Contracts.API.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

    }
    partial interface ISqlAccess {
        Task<Replacement.Contracts.API.Operation> ExecuteOperationInsertAsync(Replacement.Contracts.API.Operation args);
        Task<Replacement.Contracts.API.Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args);
        Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.API.Project args);
        Task<List<Replacement.Contracts.API.Project>> ExecuteProjectSelectAllAsync();
        Task<Replacement.Contracts.API.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args);
        Task<Replacement.Contracts.API.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.API.Project args);
        Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.API.ToDo args);
        Task<List<Replacement.Contracts.API.ToDo>> ExecuteToDoSelectAllAsync();
        Task<Replacement.Contracts.API.ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args);
        Task<List<Replacement.Contracts.API.ToDo>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.API.ToDoPK args);
        Task<Replacement.Contracts.API.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.API.ToDo args);
        Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.API.User args);
        Task<Replacement.Contracts.API.User?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.API.UserSelectByUserNameArg args);
        Task<Replacement.Contracts.API.User?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args);
        Task<Replacement.Contracts.API.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.API.User args);
    }
}

#endif
