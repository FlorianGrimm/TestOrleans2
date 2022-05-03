using Replacement.Contracts.Entity;

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
    public async Task<ActionResult<UserAPI?>> Get() {
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (UserAPI?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            return user.ToAPI();
        }
    }

    // GET api/Me
    [HttpGet("Project")]
    public async Task<ActionResult<IEnumerable<ProjectAPI>>> GetMeProject() {
        var requestOperation = this.CreateRequestOperation(
            pk: string.Empty,
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
            var result = await this.Client.GetProjectCollectionGrain().GetUsersProjects(user, operation);
            return result.ToListProjectAPI();
        }
    }

    // GET api/Me
    [HttpGet("ToDo")]
    public async Task<ActionResult<IEnumerable<ToDoAPI>>> GetMeToDo() {
        var requestOperation = this.CreateRequestOperation(
            pk: "",
            argument: (ToDoEntity?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetToDoCollectionGrain().GetUsersToDos(user, operation);
            return result.ToListToDoAPI();
        }
    }
}