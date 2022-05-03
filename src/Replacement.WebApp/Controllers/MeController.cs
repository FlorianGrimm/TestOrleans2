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
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (User?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

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
        var requestOperation = this.CreateRequestOperation(
            pk: string.Empty,
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
            return await this.Client.GetProjectCollectionGrain().GetUsersProjects(user, operation);
        }
    }

    // GET api/Me
    [HttpGet("ToDo")]
    public async Task<ActionResult<IEnumerable<ToDo>>> GetMeToDo() {
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (ToDo?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            return await this.Client.GetToDoCollectionGrain().GetUsersToDos(user, operation);
        }
    }
}