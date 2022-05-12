#if true
#nullable enable


namespace TestOrleans2.Repository.Service {
    partial class SqlAccess {
        public async Task<TestOrleans2.Contracts.Entity.OperationEntity> ExecuteOperationInsertAsync(TestOrleans2.Contracts.Entity.OperationEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleAsync<TestOrleans2.Contracts.Entity.OperationEntity>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected TestOrleans2.Contracts.Entity.OperationEntity ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationEntity(
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

        public async Task<List<TestOrleans2.Contracts.Entity.OperationEntity>> ExecuteOperationSelectAllAsync(TestOrleans2.Contracts.API.OperationFilter args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectAll]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAtLow", args.CreatedAtLow);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAtHigh", args.CreatedAtHigh);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.OperationEntity>(cmd, ReadRecordOperationSelectAll);
            }
        } 

        protected TestOrleans2.Contracts.Entity.OperationEntity ReadRecordOperationSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.OperationEntity?> ExecuteOperationSelectPKAsync(TestOrleans2.Contracts.API.OperationPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                return await this.CommandQuerySingleOrDefaultAsync<TestOrleans2.Contracts.Entity.OperationEntity>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected TestOrleans2.Contracts.Entity.OperationEntity ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationEntity(
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

        public async Task<List<TestOrleans2.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(TestOrleans2.Contracts.Entity.ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.API.ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected TestOrleans2.Contracts.API.ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.API.ProjectPK(
                @ProjectId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<TestOrleans2.Contracts.Entity.ProjectEntity>> ExecuteProjectSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.ProjectEntity>(cmd, ReadRecordProjectSelectAll);
            }
        } 

        protected TestOrleans2.Contracts.Entity.ProjectEntity ReadRecordProjectSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ProjectEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(TestOrleans2.Contracts.API.ProjectPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                System.Collections.Generic.List<TestOrleans2.Contracts.Entity.ProjectEntity> result_Projects = new System.Collections.Generic.List<TestOrleans2.Contracts.Entity.ProjectEntity>();
                System.Collections.Generic.List<TestOrleans2.Contracts.Entity.ToDoEntity> result_ToDos = new System.Collections.Generic.List<TestOrleans2.Contracts.Entity.ToDoEntity>();
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_Projects = await this.CommandReadQueryAsync<TestOrleans2.Contracts.Entity.ProjectEntity>(reader, ReadRecordProjectSelectPK_0);
                    }
                    if (idx == 1) {
                        result_ToDos = await this.CommandReadQueryAsync<TestOrleans2.Contracts.Entity.ToDoEntity>(reader, ReadRecordProjectSelectPK_1);
                    }
                } , 2);
                var result = new TestOrleans2.Contracts.Entity.ProjectSelectPKResult(
                    Projects: result_Projects,
                    ToDos: result_ToDos
                );
                return result;
            }
        } 

        protected TestOrleans2.Contracts.Entity.ProjectEntity ReadRecordProjectSelectPK_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ProjectEntity(
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

        protected TestOrleans2.Contracts.Entity.ToDoEntity ReadRecordProjectSelectPK_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ToDoEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.ProjectManipulationResult> ExecuteProjectUpsertAsync(TestOrleans2.Contracts.Entity.ProjectEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                TestOrleans2.Contracts.Entity.ProjectEntity result_DataResult = default!;
                TestOrleans2.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.ProjectEntity>(reader, ReadRecordProjectUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.OperationResult>(reader, ReadRecordProjectUpsert_1);
                    }
                } , 2);
                var result = new TestOrleans2.Contracts.Entity.ProjectManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected TestOrleans2.Contracts.Entity.ProjectEntity ReadRecordProjectUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ProjectEntity(
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

        protected TestOrleans2.Contracts.Entity.OperationResult ReadRecordProjectUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationResult(
                @resultValue: (TestOrleans2.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task ExecuteRequestLogInsertAsync(TestOrleans2.Contracts.Entity.RequestLogEntity args)  {
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

        public async Task<List<TestOrleans2.Contracts.Entity.RequestLogEntity>> ExecuteRequestLogSelectAllAsync(TestOrleans2.Contracts.API.RequestLogFilter args)  {
            using(var cmd = this.CreateCommand("[dbo].[RequestLogSelectAll]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@RequestLogId", args.RequestLogId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterString(cmd, "@ActivityId", SqlDbType.VarChar, 200, args.ActivityId);
                this.AddParameterString(cmd, "@OperationName", SqlDbType.VarChar, 100, args.OperationName);
                this.AddParameterString(cmd, "@EntityType", SqlDbType.VarChar, 50, args.EntityType);
                this.AddParameterString(cmd, "@EntityId", SqlDbType.NVarChar, 100, args.EntityId);
                this.AddParameterString(cmd, "@Argument", SqlDbType.NVarChar, -1, args.Argument);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAtLow", args.CreatedAtLow);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAtHigh", args.CreatedAtHigh);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.RequestLogEntity>(cmd, ReadRecordRequestLogSelectAll);
            }
        } 

        protected TestOrleans2.Contracts.Entity.RequestLogEntity ReadRecordRequestLogSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.RequestLogEntity(
                @RequestLogId: this.ReadGuid(reader, 0),
                @OperationId: this.ReadGuid(reader, 1),
                @ActivityId: this.ReadString(reader, 2),
                @OperationName: this.ReadString(reader, 3),
                @EntityType: this.ReadString(reader, 4),
                @EntityId: this.ReadString(reader, 5),
                @Argument: this.ReadString(reader, 6),
                @CreatedAt: this.ReadDateTimeOffset(reader, 7),
                @UserId: this.ReadGuidQ(reader, 8),
                @EntityVersion: this.ReadInt64(reader, 9)
            );
            return result;
        } 

        public async Task<List<TestOrleans2.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(TestOrleans2.Contracts.Entity.ToDoEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.API.ToDoPK>(cmd, ReadRecordToDoDeletePK);
            }
        } 

        protected TestOrleans2.Contracts.API.ToDoPK ReadRecordToDoDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.API.ToDoPK(
                @ProjectId: this.ReadGuid(reader, 0),
                @ToDoId: this.ReadGuid(reader, 1)
            );
            return result;
        } 

        public async Task<List<TestOrleans2.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectAll);
            }
        } 

        protected TestOrleans2.Contracts.Entity.ToDoEntity ReadRecordToDoSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ToDoEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.ToDoEntity?> ExecuteToDoSelectPKAsync(TestOrleans2.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQuerySingleOrDefaultAsync<TestOrleans2.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected TestOrleans2.Contracts.Entity.ToDoEntity ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ToDoEntity(
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

        public async Task<List<TestOrleans2.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectProjectAsync(TestOrleans2.Contracts.API.ToDoPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectProject]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@ToDoId", args.ToDoId);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.ToDoEntity>(cmd, ReadRecordToDoSelectProject);
            }
        } 

        protected TestOrleans2.Contracts.Entity.ToDoEntity ReadRecordToDoSelectProject(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ToDoEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.ToDoManipulationResult> ExecuteToDoUpsertAsync(TestOrleans2.Contracts.Entity.ToDoEntity args)  {
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
                TestOrleans2.Contracts.Entity.ToDoEntity result_DataResult = default!;
                TestOrleans2.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.ToDoEntity>(reader, ReadRecordToDoUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.OperationResult>(reader, ReadRecordToDoUpsert_1);
                    }
                } , 2);
                var result = new TestOrleans2.Contracts.Entity.ToDoManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected TestOrleans2.Contracts.Entity.ToDoEntity ReadRecordToDoUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.ToDoEntity(
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

        protected TestOrleans2.Contracts.Entity.OperationResult ReadRecordToDoUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationResult(
                @resultValue: (TestOrleans2.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

        public async Task<List<TestOrleans2.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(TestOrleans2.Contracts.Entity.UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                return await this.CommandQueryAsync<TestOrleans2.Contracts.API.UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected TestOrleans2.Contracts.API.UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.API.UserPK(
                @UserId: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<List<TestOrleans2.Contracts.Entity.UserEntity>> ExecuteUserSelectAllAsync()  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectAll]", CommandType.StoredProcedure)) {
                return await this.CommandQueryAsync<TestOrleans2.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectAll);
            }
        } 

        protected TestOrleans2.Contracts.Entity.UserEntity ReadRecordUserSelectAll(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.UserEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.UserEntity?> ExecuteUserSelectByUserNameAsync(TestOrleans2.Contracts.Entity.UserSelectByUserNameArg args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<TestOrleans2.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected TestOrleans2.Contracts.Entity.UserEntity ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.UserEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.UserEntity?> ExecuteUserSelectPKAsync(TestOrleans2.Contracts.API.UserPK args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                return await this.CommandQuerySingleOrDefaultAsync<TestOrleans2.Contracts.Entity.UserEntity>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected TestOrleans2.Contracts.Entity.UserEntity ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.UserEntity(
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

        public async Task<TestOrleans2.Contracts.Entity.UserManipulationResult> ExecuteUserUpsertAsync(TestOrleans2.Contracts.Entity.UserEntity args)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure)) {
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@CreatedBy", args.CreatedBy);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterGuid(cmd, "@ModifiedBy", args.ModifiedBy);
                this.AddParameterLong(cmd, "@EntityVersion", args.EntityVersion);
                TestOrleans2.Contracts.Entity.UserEntity result_DataResult = default!;
                TestOrleans2.Contracts.Entity.OperationResult result_OperationResult = default!;
                await this.CommandQueryMultipleAsync(cmd, async (idx, reader) => {
                    if (idx == 0) {
                        result_DataResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.UserEntity>(reader, ReadRecordUserUpsert_0);
                    }
                    if (idx == 1) {
                        result_OperationResult = await this.CommandReadQuerySingleAsync<TestOrleans2.Contracts.Entity.OperationResult>(reader, ReadRecordUserUpsert_1);
                    }
                } , 2);
                var result = new TestOrleans2.Contracts.Entity.UserManipulationResult(
                    DataResult: result_DataResult,
                    OperationResult: result_OperationResult
                );
                return result;
            }
        } 

        protected TestOrleans2.Contracts.Entity.UserEntity ReadRecordUserUpsert_0(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.UserEntity(
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

        protected TestOrleans2.Contracts.Entity.OperationResult ReadRecordUserUpsert_1(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new TestOrleans2.Contracts.Entity.OperationResult(
                @resultValue: (TestOrleans2.Contracts.Entity.ResultValue) (this.ReadInt32(reader, 0))
            ) {
                Message = this.ReadString(reader, 1)
            } ;
            return result;
        } 

    }
    partial interface ISqlAccess {
        Task<TestOrleans2.Contracts.Entity.OperationEntity> ExecuteOperationInsertAsync(TestOrleans2.Contracts.Entity.OperationEntity args);
        Task<List<TestOrleans2.Contracts.Entity.OperationEntity>> ExecuteOperationSelectAllAsync(TestOrleans2.Contracts.API.OperationFilter args);
        Task<TestOrleans2.Contracts.Entity.OperationEntity?> ExecuteOperationSelectPKAsync(TestOrleans2.Contracts.API.OperationPK args);
        Task<List<TestOrleans2.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(TestOrleans2.Contracts.Entity.ProjectEntity args);
        Task<List<TestOrleans2.Contracts.Entity.ProjectEntity>> ExecuteProjectSelectAllAsync();
        Task<TestOrleans2.Contracts.Entity.ProjectSelectPKResult> ExecuteProjectSelectPKAsync(TestOrleans2.Contracts.API.ProjectPK args);
        Task<TestOrleans2.Contracts.Entity.ProjectManipulationResult> ExecuteProjectUpsertAsync(TestOrleans2.Contracts.Entity.ProjectEntity args);
        Task ExecuteRequestLogInsertAsync(TestOrleans2.Contracts.Entity.RequestLogEntity args);
        Task<List<TestOrleans2.Contracts.Entity.RequestLogEntity>> ExecuteRequestLogSelectAllAsync(TestOrleans2.Contracts.API.RequestLogFilter args);
        Task<List<TestOrleans2.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(TestOrleans2.Contracts.Entity.ToDoEntity args);
        Task<List<TestOrleans2.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectAllAsync();
        Task<TestOrleans2.Contracts.Entity.ToDoEntity?> ExecuteToDoSelectPKAsync(TestOrleans2.Contracts.API.ToDoPK args);
        Task<List<TestOrleans2.Contracts.Entity.ToDoEntity>> ExecuteToDoSelectProjectAsync(TestOrleans2.Contracts.API.ToDoPK args);
        Task<TestOrleans2.Contracts.Entity.ToDoManipulationResult> ExecuteToDoUpsertAsync(TestOrleans2.Contracts.Entity.ToDoEntity args);
        Task<List<TestOrleans2.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(TestOrleans2.Contracts.Entity.UserEntity args);
        Task<List<TestOrleans2.Contracts.Entity.UserEntity>> ExecuteUserSelectAllAsync();
        Task<TestOrleans2.Contracts.Entity.UserEntity?> ExecuteUserSelectByUserNameAsync(TestOrleans2.Contracts.Entity.UserSelectByUserNameArg args);
        Task<TestOrleans2.Contracts.Entity.UserEntity?> ExecuteUserSelectPKAsync(TestOrleans2.Contracts.API.UserPK args);
        Task<TestOrleans2.Contracts.Entity.UserManipulationResult> ExecuteUserUpsertAsync(TestOrleans2.Contracts.Entity.UserEntity args);
    }
}

#endif
