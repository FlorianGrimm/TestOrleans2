namespace Replacement.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ReplacementControllerBase {
    public ProjectController(
         IClusterClient client,
         ILogger<UserController> logger
         )
         : base(client, logger) {
    }

    // GET: api/Project
    [HttpGet(Name = "ProjectGetAll")]
    public async Task<ActionResult<IEnumerable<ProjectAPI>>> Get() {
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (ProjectAPI?)null
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
            return result.ToListProjectAPI();
        }
    }

    // GET api/Project/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{projectId}", Name = "ProjectGetOne")]
    public async Task<ActionResult<ProjectAPI?>> Get(Guid projectId) {
        var requestOperation = this.CreateRequestOperation(
            pk: projectId,
            argument: (ProjectAPI?)null
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
            return result.ToProjectAPI();
        }
    }

    // POST api/Project
    [HttpPost(Name = "ProjectPost")]
    public async Task<ActionResult<ProjectAPI?>> Post([FromBody] ProjectAPI value) {
        if (value.ProjectId == Guid.Empty) {
            value = value with {
                ProjectId = Guid.NewGuid(),
                SerialVersion = 0
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
            if (result is not null) {
                return result.ToProjectAPI();
                //return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/Project/5
    [HttpPut("{projectId}", Name = "ProjectPut")]
    public async Task<ActionResult<ProjectAPI?>> Put(Guid projectId, [FromBody] ProjectAPI value) {
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
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var project = await grain.UpsertProject(value.ToProjectEntity(), user, operation);
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
            if (project is not null) {
                return project.ToProjectAPI();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/Project/5
    [HttpDelete("{projectId}", Name = "ProjectDelete")]
    public async Task<ActionResult> Delete(Guid projectId) {
        if (projectId == Guid.Empty) {
            return this.NotFound();
        }

        var requestOperation = this.CreateRequestOperation(
            pk: projectId,
            argument: (ProjectAPI?)null
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
