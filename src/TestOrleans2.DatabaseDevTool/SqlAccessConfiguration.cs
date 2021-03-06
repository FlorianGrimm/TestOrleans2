using Brimborium.TypedStoredProcedure;

using TestOrleans2.Contracts.Entity;

namespace TestOrleans2.DatabaseDevTool;
public static partial class Program {
    private static void AddNativeTypeConverter() {
        SQLUtility.AddDefaultTypeConverter();
        //SQLUtility.AddTypeConverter(
        //    typeof(int),
        //    typeof(bool),
        //    typeof(Solvin.OneTS.Services.SqlHelpers.Int32Converter),
        //    nameof(Solvin.OneTS.Services.SqlHelpers.Int32Converter.ToBool));
    }

    //private static MemberDefinition TypeOfResultAsync<TRecord>() {
    //    return new MemberDefinition(
    //            nameof(IDataManipulationResult<TRecord>.OperationResult),
    //            CSTypeDefinition.TypeOf<OperationResult>(
    //                isAsync:true,
    //                isList:false)
    //        );
    //}


    private static CSTypeDefinition TypeOfResultAsync<TResult, TRecord1, TRecord2>(
        string result1,
        bool isList1,
        string result2,
        bool isList2
        ) {
        return CSTypeDefinition.TypeOf<TResult>(
            members: new MemberDefinition[] {
            new MemberDefinition(
                result1,
                CSTypeDefinition.TypeOf<TRecord1>(
                    isAsync:true,
                    isList:isList1)
                ),
            new MemberDefinition(
                result2,
                CSTypeDefinition.TypeOf<TRecord2>(
                    isAsync:true,
                    isList:isList2)
                )
            },
            isAsync: true);
    }

    private static CSTypeDefinition TypeOfTManipulationResult<TManipulationResult, TRecord>() {
        return CSTypeDefinition.TypeOf<TManipulationResult>(
            members: new MemberDefinition[] {
            new MemberDefinition(
                nameof(IDataManipulationResult<TRecord>.DataResult),
                CSTypeDefinition.TypeOf<TRecord>(
                    isAsync:true,
                    isList:false)
                ),
            new MemberDefinition(
                nameof(IDataManipulationResult<TRecord>.OperationResult),
                CSTypeDefinition.TypeOf<OperationResult>(
                    isAsync:true,
                    isList:false)
                )
            },
            isAsync: true);
    }
    public static DatabaseDefintion GetDefintion() {
        return new DatabaseDefintion(
            StoredProcedures: new StoredProcedureDefintion[] {
                new StoredProcedureDefintion("dbo", "OperationInsert",
                    CSTypeDefinition.TypeOf<OperationEntity>(),
                    ExecutionMode.QuerySingle,
                    CSTypeDefinition.TypeOf<OperationEntity>(isList:false, isAsync: true)),

                new StoredProcedureDefintion("dbo", "OperationSelectPK",
                    CSTypeDefinition.TypeOf<OperationPK>(),
                    ExecutionMode.QuerySingleOrDefault,
                    CSTypeDefinition.TypeOf<OperationEntity>(isList:false, isAsync: true)),

                 new StoredProcedureDefintion("dbo", "OperationSelectAll", 
                     CSTypeDefinition.TypeOf<OperationFilter>(), 
                     ExecutionMode.Query,
                     CSTypeDefinition.TypeOf<OperationEntity>(isList:true, isAsync: true)),

                new StoredProcedureDefintion("dbo", "RequestLogInsert", 
                    CSTypeDefinition.TypeOf<RequestLogEntity>(), 
                    ExecutionMode.ExecuteNonQuery, 
                    CSTypeDefinition.VoidAsync),

                new StoredProcedureDefintion("dbo", "RequestLogSelectAll", 
                    CSTypeDefinition.TypeOf<RequestLogFilter>(), 
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<RequestLogEntity>(isList:true, isAsync: true)),

                new StoredProcedureDefintion("dbo", "ProjectDeletePK",
                    CSTypeDefinition.TypeOf<ProjectEntity>(),
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<ProjectPK>(isList:true, isAsync:true)),

                new StoredProcedureDefintion("dbo", "ProjectSelectPK",
                    CSTypeDefinition.TypeOf<ProjectPK>(),
                    ExecutionMode.QueryMultiple,
                    TypeOfResultAsync<ProjectSelectPKResult, ProjectEntity, ToDoEntity>(nameof(ProjectSelectPKResult.Projects), true, nameof(ProjectSelectPKResult.ToDos), true)
                    //CSTypeDefinition.TypeOf<Project>(isList:false, isAsync:true)
                    ),
                new StoredProcedureDefintion("dbo", "ProjectSelectAll",
                    CSTypeDefinition.Void,
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<ProjectEntity>(isList:true, isAsync:true)
                    ),

                new StoredProcedureDefintion("dbo", "ProjectUpsert",
                    CSTypeDefinition.TypeOf<ProjectEntity>(),
                    ExecutionMode.QueryMultiple,
                    TypeOfTManipulationResult<ProjectManipulationResult, ProjectEntity>()),

                new StoredProcedureDefintion("dbo", "ToDoDeletePK",
                    CSTypeDefinition.TypeOf<ToDoEntity>(),
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<ToDoPK>(isList:true, isAsync:true)),
                new StoredProcedureDefintion("dbo", "ToDoSelectPK",
                    CSTypeDefinition.TypeOf<ToDoPK>(),
                    ExecutionMode.QuerySingleOrDefault,
                    CSTypeDefinition.TypeOf<ToDoEntity>(isList:false, isAsync:true)),
                new StoredProcedureDefintion("dbo", "ToDoSelectProject",
                    CSTypeDefinition.TypeOf<ToDoPK>(),
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<ToDoEntity>(isList:true, isAsync:true)),
                new StoredProcedureDefintion("dbo", "ToDoUpsert",
                    CSTypeDefinition.TypeOf<ToDoEntity>(),
                    ExecutionMode.QueryMultiple,
                    TypeOfTManipulationResult<ToDoManipulationResult, ToDoEntity>()),

                new StoredProcedureDefintion("dbo", "ToDoSelectAll",
                    CSTypeDefinition.Void,
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<ToDoEntity>(isList:true, isAsync:true)),


                new StoredProcedureDefintion("dbo", "UserDeletePK",
                    CSTypeDefinition.TypeOf<UserEntity>(),
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<UserPK>(isList:true, isAsync:true)),
                new StoredProcedureDefintion("dbo", "UserSelectAll",
                    CSTypeDefinition.Void,
                    ExecutionMode.Query,
                    CSTypeDefinition.TypeOf<UserEntity>(isList:true, isAsync:true)),
                new StoredProcedureDefintion("dbo", "UserSelectPK",
                    CSTypeDefinition.TypeOf<UserPK>(),
                    ExecutionMode.QuerySingleOrDefault,
                    CSTypeDefinition.TypeOf<UserEntity>(isList:false, isAsync:true)),
                new StoredProcedureDefintion("dbo", "UserUpsert",
                    CSTypeDefinition.TypeOf<UserEntity>(),
                    ExecutionMode.QueryMultiple,
                    TypeOfTManipulationResult<UserManipulationResult, UserEntity>()),

                new StoredProcedureDefintion("dbo", "UserSelectByUserName",
                    CSTypeDefinition.TypeOf<UserSelectByUserNameArg>(),
                    ExecutionMode.QuerySingleOrDefault,
                    CSTypeDefinition.TypeOf<UserEntity>(isList:false, isAsync:true)),

                new StoredProcedureDefintion("dbo", "EnableReadCommittedSnapshot", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "OrleansQueryFill", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "OrleansStorageEnableDataCompression", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "RequestLogSelectPK", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),

                new StoredProcedureDefintion("dbo", "sp_alterdiagram", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_creatediagram", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_dropdiagram", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_helpdiagramdefinition", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_helpdiagrams", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_renamediagram", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
                new StoredProcedureDefintion("dbo", "sp_upgraddiagrams", CSTypeDefinition.None, ExecutionMode.Ignore, CSTypeDefinition.None),
             },
             IgnoreTypePropertyNames: new TypePropertyNames[] {
             }
        );
    }
}
