namespace Replacement.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeController : ReplacementControllerBase {
    public MeController(
        IClusterClient client,
        ILogger<UserController> logger
        )
        : base(client, logger) {
    }

    // GET api/Me
    [HttpGet()]
    public async Task<ActionResult<User?>> Get() {
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(User),
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
            return user;
        }
    }

    // GET api/Me
    [HttpGet("Project")]
    public async Task<ActionResult<IEnumerable<Project>>> GetMeProject() {
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(User),
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
            return await this.Client.GetProjectCollectionGrain().GetUsersProjects(user, operation);
        }
    }

    // GET api/Me
    [HttpGet("ToDo")]
    public async Task<ActionResult<IEnumerable<ToDo>>> GetMeToDo() {
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(User),
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
            return await this.Client.GetToDoCollectionGrain().GetUsersToDos(user, operation);
        }
    }
}