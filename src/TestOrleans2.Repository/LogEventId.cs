namespace TestOrleans2.Repository;

internal enum LogEventId {
    Replacement_Repository_Start = 1000,
    ProjectCollectionGrain_LogSubscripe,
    ProjectCollectionGrain_LogUnsubscripe,
    ProjectCollectionGrain_LogSetDirtyProject,
    ProjectCollectionGrain_LogSetDirtyToDo,
    ProjectCollectionGrain_GetAllProjects,
    ProjectCollectionGrain_GetUsersProjects,
    ProjectGrain_Load,
    ProjectGrain_GetProject
}

