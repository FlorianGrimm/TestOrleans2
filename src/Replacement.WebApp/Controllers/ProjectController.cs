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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> Get() {
        var operation = new Operation(
               OperationId: Guid.NewGuid(),
               Title: this.GetOperationTitle(),
               EntityType: nameof(Project),
               EntityId: "",
               Data: this.GetOperationData(),
               UserId: null,
               CreatedAt: DateTimeOffset.Now,
               SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IProjectCollectionGrain>(Guid.Empty)!;
            return await grain.GetAllProjects(user, operation);
        }
    }

    // GET api/Project/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{projectId}")]
    public async Task<ActionResult<Project?>> Get(Guid projectId) {
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(Project),
                EntityId: projectId.ToString(),
                Data: this.GetOperationData(),
                UserId: null,
                CreatedAt: DateTimeOffset.Now,
                SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(projectId);
            var result = await grain.GetProject(user, operation);
            return result;
        }
    }

    // POST api/Project
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Project value) {
        if (value.ProjectId == Guid.Empty) {
            value = value with {
                ProjectId = Guid.NewGuid()
            };
        }
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(Project),
            EntityId: value.ProjectId.ToString(),
            Data: this.GetOperationData(),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var result = await grain.UpsertProject(value, user, operation);
            if (result is not null) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/Project/5
    [HttpPut("{projectId}")]
    public async Task<ActionResult> Put(Guid projectId, [FromBody] Project value) {
        value = value with { ProjectId = projectId };
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(Project),
            EntityId: projectId.ToString(),
            Data: this.GetOperationData(),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(value.ProjectId);
            var project = await grain.UpsertProject(value, user, operation);
            if (project is not null) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/Project/5
    [HttpDelete("{projectId}")]
    public async Task<ActionResult> Delete(Guid projectId) {
        if (projectId == Guid.Empty) {
            return NotFound();
        }
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(Project),
            EntityId: projectId.ToString(),
            Data: this.GetOperationData(),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetProjectGrain(projectId).DeleteProject(user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
