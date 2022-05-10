#if true
#nullable enable


namespace Replacement.Repository.Service {
    partial class SqlAccess {
        public async Task<Replacement.Contracts.Entity.OperationEntity> ExecuteOperationInsertAsync(Replacement.Contracts.Entity.OperationEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleAsync<Replacement.Contracts.Entity.OperationEntity>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected Replacement.Contracts.Entity.OperationEntity ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.OperationEntity(
                @OperationId: this.ReadGuid(reader, 0),
                @OperationName: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @CreatedAt: this.ReadDateTimeOffset(reader, 4),
                @UserId: this.ReadGuidQ(reader, 5),
                @EntityVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.OperationEntity?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.OperationEntity>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected Replacement.Contracts.Entity.OperationEntity ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.OperationEntity(
                @OperationId: this.ReadGuid(reader, 0),
                @OperationName: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @CreatedAt: this.ReadDateTimeOffset(reader, 4),
                @UserId: this.ReadGuidQ(reader, 5),
                @EntityVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.Entity.ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected Replacement.Contracts.API.ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ProjectPK(
                @ProjectId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.Entity.ProjectEntity>> ExecuteProjectSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ProjectEntity>(cmd, ReadRecordProjectSelectAll);
            }
        } 

        protected Replacement.Contracts.Entity.ProjectEntity ReadRecordProjectSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                System.Collections.Generic.List<Replacement.Contracts.Entity.ProjectEntity> result_Projects = new System.Collections.Generic.List<Replacement.Contracts.Entity.ProjectEntity>();
                System.Collections.Generic.List<Replacement.Contracts.Entity.ToDoEntity> result_ToDos = new System.Collections.Generic.List<Replacement.Contracts.Entity.ToDoEntity>();
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_Projects = await this.CommandReadQueryAsync<Replacement.Contracts.Entity.ProjectEntity>(reader, ReadRecordProjectSelectPK_0);
                    }
                    if (idx == 1) {
                        result_ToDos = await this.CommandReadQueryAsync<Replacement.Contracts.Entity.ToDoEntity>(reader, ReadRecordProjectSelectPK_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ProjectSelectPKResult(
                    Projects: result_Projects,
                    ToDos: result_ToDos
                );
                return result;
            }
        } 

        protected Replacement.Contracts.Entity.ProjectEntity ReadRecordProjectSelectPK_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected Replacement.Contracts.Entity.ToDoEntity ReadRecordProjectSelectPK_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoEntity(
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
                @EntityVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.Entity.ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                Replacement.Contracts.Entity.ProjectEntity result_DataResult = default!;
                Replacement.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.ProjectEntity>(reader, ReadRecordProjectUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.OperationResult>(reader, ReadRecordProjectUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ProjectManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.Entity.ProjectEntity ReadRecordProjectUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ProjectEntity(
                @ProjectId: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected Replacement.Contracts.Entity.OperationResult ReadRecordProjectUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (Replacement.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task ExecuteRequestLogInsertAsync(Replacement.Contracts.Entity.RequestLogEntity args)  {
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

        public async Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.Entity.ToDoEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
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

        public async Task<List<Replacement.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectAll);
            }
        } 

        protected Replacement.Contracts.Entity.ToDoEntity ReadRecordToDoSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoEntity(
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
                @EntityVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.ToDoEntity?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected Replacement.Contracts.Entity.ToDoEntity ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoEntity(
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
                @EntityVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectProject]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectProject);
            }
        } 

        protected Replacement.Contracts.Entity.ToDoEntity ReadRecordToDoSelectProject(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoEntity(
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
                @EntityVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.Entity.ToDoEntity args)  {
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
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                Replacement.Contracts.Entity.ToDoEntity result_DataResult = default!;
                Replacement.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.ToDoEntity>(reader, ReadRecordToDoUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.OperationResult>(reader, ReadRecordToDoUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.ToDoManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.Entity.ToDoEntity ReadRecordToDoUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.ToDoEntity(
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
                @EntityVersion: this.ReadInt64(reader, 10)
            );
            return result;
        } 

        protected Replacement.Contracts.Entity.OperationResult ReadRecordToDoUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (Replacement.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.Entity.UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected Replacement.Contracts.API.UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.UserPK(
                @UserId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.Entity.UserEntity>> ExecuteUserSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<Replacement.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectAll);
            }
        } 

        protected Replacement.Contracts.Entity.UserEntity ReadRecordUserSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.UserEntity?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.Entity.UserSelectByUserNameArg args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected Replacement.Contracts.Entity.UserEntity ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.UserEntity?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected Replacement.Contracts.Entity.UserEntity ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.Entity.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.Entity.UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                Replacement.Contracts.Entity.UserEntity result_DataResult = default!;
                Replacement.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.UserEntity>(reader, ReadRecordUserUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<Replacement.Contracts.Entity.OperationResult>(reader, ReadRecordUserUpsert_1);
                    }
                } , 2);
                var result = new Replacement.Contracts.Entity.UserManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected Replacement.Contracts.Entity.UserEntity ReadRecordUserUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.UserEntity(
                @UserId: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuid(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @CreatedBy: this.ReadGuidQ(reader, 4),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 5),
                @ModifiedBy: this.ReadGuidQ(reader, 6),
                @EntityVersion: this.ReadInt64(reader, 7)
            );
            return result;
        } 

        protected Replacement.Contracts.Entity.OperationResult ReadRecordUserUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.Entity.OperationResult(
                @resultValue: (Replacement.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

    }
    partial interface ISqlAccess {
        Task<Replacement.Contracts.Entity.OperationEntity> ExecuteOperationInsertAsync(Replacement.Contracts.Entity.OperationEntity args);
        Task<Replacement.Contracts.Entity.OperationEntity?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args);
        Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.Entity.ProjectEntity args);
        Task<List<Replacement.Contracts.Entity.ProjectEntity>> ExecuteProjectSelectAllAsync();
        Task<Replacement.Contracts.Entity.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args);
        Task<Replacement.Contracts.Entity.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.Entity.ProjectEntity args);
        Task ExecuteRequestLogInsertAsync(Replacement.Contracts.Entity.RequestLogEntity args);
        Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.Entity.ToDoEntity args);
        Task<List<Replacement.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectAllAsync();
        Task<Replacement.Contracts.Entity.ToDoEntity?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args);
        Task<List<Replacement.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectProjectAsync(Replacement.Contracts.API.ToDoPK args);
        Task<Replacement.Contracts.Entity.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.Entity.ToDoEntity args);
        Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.Entity.UserEntity args);
        Task<List<Replacement.Contracts.Entity.UserEntity>> ExecuteUserSelectAllAsync();
        Task<Replacement.Contracts.Entity.UserEntity?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.Entity.UserSelectByUserNameArg args);
        Task<Replacement.Contracts.Entity.UserEntity?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args);
        Task<Replacement.Contracts.Entity.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.Entity.UserEntity args);
    }
}

#endif
