#if true
#nullable enable


namespace Replacement.Repository.Service {
    partial class SqlAccess {
        public async Task<Replacement.Contracts.API.Operation> ExecuteOperationInsertAsync(Replacement.Contracts.API.Operation args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationInsert]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 20, args.Title);
                this.AddParameterString(cmd, "@Data", SqlDbType.NVarChar, -1, args.Data);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                return await this.CommandQuerySingleAsync<Replacement.Contracts.API.Operation>(cmd, ReadRecordOperationInsert);
            }
        } 

        protected Replacement.Contracts.API.Operation ReadRecordOperationInsert(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Operation(
                @Id: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @Data: this.ReadString(reader, 4),
                @CreatedAt: this.ReadDateTimeOffset(reader, 5),
                @SerialVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[OperationSelectPK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterGuid(cmd, "@Id", args.Id);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.Operation>(cmd, ReadRecordOperationSelectPK);
            }
        } 

        protected Replacement.Contracts.API.Operation ReadRecordOperationSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Operation(
                @Id: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @EntityType: this.ReadString(reader, 2),
                @EntityId: this.ReadString(reader, 3),
                @Data: this.ReadString(reader, 4),
                @CreatedAt: this.ReadDateTimeOffset(reader, 5),
                @SerialVersion: this.ReadInt64(reader, 6)
            );
            return result;
        } 

        public async Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.API.Project args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectDeletePK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ProjectPK>(cmd, ReadRecordProjectDeletePK);
            }
        } 

        protected Replacement.Contracts.API.ProjectPK ReadRecordProjectDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ProjectPK(
                @Id: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.Project?> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectSelectPK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.Project>(cmd, ReadRecordProjectSelectPK);
            }
        } 

        protected Replacement.Contracts.API.Project ReadRecordProjectSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.Project(
                @Id: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuidQ(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 4),
                @SerialVersion: this.ReadInt64(reader, 5)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.API.Project args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ProjectUpsert]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
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
                @Id: this.ReadGuid(reader, 0),
                @Title: this.ReadString(reader, 1),
                @OperationId: this.ReadGuidQ(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 4),
                @SerialVersion: this.ReadInt64(reader, 5)
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

        public async Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.API.ToDo args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoDeletePK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.ToDoPK>(cmd, ReadRecordToDoDeletePK);
            }
        } 

        protected Replacement.Contracts.API.ToDoPK ReadRecordToDoDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDoPK(
                @Id: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoSelectPK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.ToDo>(cmd, ReadRecordToDoSelectPK);
            }
        } 

        protected Replacement.Contracts.API.ToDo ReadRecordToDoSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.ToDo(
                @Id: this.ReadGuid(reader, 0),
                @ProjectId: this.ReadGuidQ(reader, 1),
                @UserId: this.ReadGuidQ(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuidQ(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 7),
                @SerialVersion: this.ReadInt64(reader, 8)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.API.ToDo args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[ToDoUpsert]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterGuid(cmd, "@ProjectId", args.ProjectId);
                this.AddParameterGuid(cmd, "@UserId", args.UserId);
                this.AddParameterString(cmd, "@Title", SqlDbType.NVarChar, 50, args.Title);
                this.AddParameterBoolean(cmd, "@Done", args.Done);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
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
                @Id: this.ReadGuid(reader, 0),
                @ProjectId: this.ReadGuidQ(reader, 1),
                @UserId: this.ReadGuidQ(reader, 2),
                @Title: this.ReadString(reader, 3),
                @Done: this.ReadBoolean(reader, 4),
                @OperationId: this.ReadGuidQ(reader, 5),
                @CreatedAt: this.ReadDateTimeOffset(reader, 6),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 7),
                @SerialVersion: this.ReadInt64(reader, 8)
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

        public async Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.API.User args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[UserDeletePK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
                this.AddParameterLong(cmd, "@SerialVersion", args.SerialVersion);
                return await this.CommandQueryAsync<Replacement.Contracts.API.UserPK>(cmd, ReadRecordUserDeletePK);
            }
        } 

        protected Replacement.Contracts.API.UserPK ReadRecordUserDeletePK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.UserPK(
                @Id: this.ReadGuid(reader, 0)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.User?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.API.UserSelectByUserNameArg args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectByUserName]", CommandType.StoredProcedure, tx)) {
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.User>(cmd, ReadRecordUserSelectByUserName);
            }
        } 

        protected Replacement.Contracts.API.User ReadRecordUserSelectByUserName(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.User(
                @Id: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuidQ(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 4),
                @SerialVersion: this.ReadInt64(reader, 5)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.User?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[UserSelectPK]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                return await this.CommandQuerySingleOrDefaultAsync<Replacement.Contracts.API.User>(cmd, ReadRecordUserSelectPK);
            }
        } 

        protected Replacement.Contracts.API.User ReadRecordUserSelectPK(Microsoft.Data.SqlClient.SqlDataReader reader) {
            var result = new Replacement.Contracts.API.User(
                @Id: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuidQ(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 4),
                @SerialVersion: this.ReadInt64(reader, 5)
            );
            return result;
        } 

        public async Task<Replacement.Contracts.API.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.API.User args, IDbTransaction? tx = null)  {
            using(var cmd = this.CreateCommand("[dbo].[UserUpsert]", CommandType.StoredProcedure, tx)) {
                this.AddParameterGuid(cmd, "@Id", args.Id);
                this.AddParameterString(cmd, "@UserName", SqlDbType.NVarChar, 50, args.UserName);
                this.AddParameterGuid(cmd, "@OperationId", args.OperationId);
                this.AddParameterDateTimeOffset(cmd, "@CreatedAt", args.CreatedAt);
                this.AddParameterDateTimeOffset(cmd, "@ModifiedAt", args.ModifiedAt);
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
                @Id: this.ReadGuid(reader, 0),
                @UserName: this.ReadString(reader, 1),
                @OperationId: this.ReadGuidQ(reader, 2),
                @CreatedAt: this.ReadDateTimeOffset(reader, 3),
                @ModifiedAt: this.ReadDateTimeOffset(reader, 4),
                @SerialVersion: this.ReadInt64(reader, 5)
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
        Task<Replacement.Contracts.API.Operation> ExecuteOperationInsertAsync(Replacement.Contracts.API.Operation args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.Operation?> ExecuteOperationSelectPKAsync(Replacement.Contracts.API.OperationPK args, IDbTransaction? tx = null) ;
        Task<List<Replacement.Contracts.API.ProjectPK>> ExecuteProjectDeletePKAsync(Replacement.Contracts.API.Project args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.Project?> ExecuteProjectSelectPKAsync(Replacement.Contracts.API.ProjectPK args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.ProjectManipulationResult> ExecuteProjectUpsertAsync(Replacement.Contracts.API.Project args, IDbTransaction? tx = null) ;
        Task<List<Replacement.Contracts.API.ToDoPK>> ExecuteToDoDeletePKAsync(Replacement.Contracts.API.ToDo args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.ToDo?> ExecuteToDoSelectPKAsync(Replacement.Contracts.API.ToDoPK args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.ToDoManipulationResult> ExecuteToDoUpsertAsync(Replacement.Contracts.API.ToDo args, IDbTransaction? tx = null) ;
        Task<List<Replacement.Contracts.API.UserPK>> ExecuteUserDeletePKAsync(Replacement.Contracts.API.User args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.User?> ExecuteUserSelectByUserNameAsync(Replacement.Contracts.API.UserSelectByUserNameArg args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.User?> ExecuteUserSelectPKAsync(Replacement.Contracts.API.UserPK args, IDbTransaction? tx = null) ;
        Task<Replacement.Contracts.API.UserManipulationResult> ExecuteUserUpsertAsync(Replacement.Contracts.API.User args, IDbTransaction? tx = null) ;
    }
}

#endif
