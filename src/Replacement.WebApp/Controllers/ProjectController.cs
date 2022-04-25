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
        var operation = new Replacement.Contracts.API.Operation(
               Guid.NewGuid(),
               this.GetOperationTitle(),
               nameof(Project),
               "",
               this.GetOperationData(),
               DateTimeOffset.Now,
               0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IProjectCollectionGrain>(Guid.Empty)!;
            return await grain.GetAllProjects(user, operation);
        }
    }

    // GET api/Project/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{id}")]
    public async Task<ActionResult<Project?>> Get(Guid id) {
        var operation = new Replacement.Contracts.API.Operation(
                Guid.NewGuid(),
                this.GetOperationTitle(),
                nameof(Project),
                id.ToString(),
                this.GetOperationData(),
                DateTimeOffset.Now,
                0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(id);
            var result = await grain.GetProject(user, operation);
            return result;
        }
    }

    // POST api/Project
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ToDo value) {
        if (value.Id == Guid.Empty) {
            value = value with {
                Id = Guid.NewGuid()
            };
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(Project),
            value.Id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(value.Id);
            var result = await grain.UpsertProject(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/Project/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] Project value) {
        value = value with { Id = id };
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(Project),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IProjectGrain>(value.Id);
            var result = await grain.UpsertProject(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/Project/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id) {
        if (id == Guid.Empty) {
            return NotFound();
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(Project),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetGrain<IProjectGrain>(id).DeleteProject(user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
