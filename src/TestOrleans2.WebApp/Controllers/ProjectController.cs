namespace TestOrleans2.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ReplacementControllerBase {
    private readonly AppPolicies _AppPolicies;

    public ProjectController(
        IClusterClient client,
        AppPolicies appPolicies,
        ILogger<UserController> logger
        ) : base(client, logger) {
        this._AppPolicies = appPolicies;
    }

    // GET: api/Project
    [HttpGet(Name = "ProjectGetAll")]
    public async Task<ActionResult<IEnumerable<Project>>> Get() {
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (Project?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IProjectCollectionGrain>(Guid.Empty)!;
            var result = await grain.GetAllProjects(user, operation);
            if (result is null) {
                return this.Forbid();
            }
            return result.ToListProject();
        }
    }

    // GET api/Project/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{projectId}", Name = "ProjectGetOne")]
    public async Task<ActionResult<Project?>> Get(Guid projectId) {
        var requestOperation = this.CreateRequestOperation(
            pk: projectId,
            argument: (Project?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(projectId);
            var result = await grain.GetProject(user, operation);
            if (result is null) {
                return this.Forbid();
            }
            return result.ToProject();
        }
    }

    // POST api/Project
    [HttpPost(Name = "ProjectPostOne")]
    public async Task<ActionResult<Project?>> Post([FromBody] Project value) {
        if (value.ProjectId == Guid.Empty) {
            value = value with {
                ProjectId = Guid.NewGuid(),
                DataVersion = string.Empty
            };
        }

        var requestOperation = this.CreateRequestOperation(
            pk: value.ProjectId,
            argument: value
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: true,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var result = await grain.UpsertProject(value.ToProjectEntity(), user, operation);
            if (result is null) {
                return this.Conflict();
            }
            return result.ToProject();

        }
    }

    // PUT api/Project/5
    [HttpPut("{projectId}", Name = "ProjectPutOne")]
    public async Task<ActionResult<Project?>> Put(Guid projectId, [FromBody] Project value) {
        value = value with { ProjectId = projectId };
        var requestOperation = this.CreateRequestOperation(
            pk: projectId,
            argument: value
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: true,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
#if false
            Project? project = null;
            for (int iWatchDog = 2; iWatchDog >= 0; iWatchDog--) {
                try {
                    project = await grain.UpsertProject(value, user, operation);
                    // when ((uint)sqlException.ErrorCode == 0x80131904)
                    //} catch (Microsoft.Data.SqlClient.SqlException sqlException)  {
                } catch (System.Exception exception) {
                    if (!System.Diagnostics.Debugger.IsAttached) {
                        System.Diagnostics.Debugger.Launch();
                    }
                    System.Diagnostics.Debugger.Break();
                    if (exception is not null) {
                        throw exception;
                    }
                    //System.Console.Out.WriteLine($"ErrorCode:{sqlException.ErrorCode}; Number:{sqlException.Number};");

                    await Task.Delay(50);
                    //} catch (Microsoft.Data.SqlClient.SqlException sqlException) {
                    //    await Task.Delay(50);
                }
            }
#endif
#if true
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var result = await grain.UpsertProject(value.ToProjectEntity(), user, operation);
            if (result is null) {
                return this.Conflict();
            }
            return result.ToProject();

#endif

#if false
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var project = await this._AppPolicies.ControllerToGrainPolicy.ExecuteAsync(async () => await grain.UpsertProject(value.ToProjectEntity(), user, operation));

            //var project = await grain.UpsertProject(value.ToProjectEntity(), user, operation);
            if (project is not null) {
                return project.ToProject();
            } else {
                return this.Conflict();
            }
#endif
        }
    }

    // DELETE api/Project/5
    [HttpDelete("{projectId}", Name = "ProjectDeleteOne")]
    public async Task<ActionResult> Delete(Guid projectId) {
        if (projectId == Guid.Empty) {
            return this.NotFound();
        }

        var requestOperation = this.CreateRequestOperation(
            pk: projectId,
            argument: (Project?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: true,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetProjectGrain(projectId).DeleteProject(user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.NotFound();
            }
        }
    }
}
