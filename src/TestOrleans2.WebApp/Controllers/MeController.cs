namespace TestOrleans2.WebApp.Controllers;

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
    [HttpGet(Name = "MeUserGetOne")]
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
            return user.ToUser();
        }
    }

    // GET api/Me
    [HttpGet("Project", Name = "MeProjectGetAll")]
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
            var result = await this.Client.GetProjectCollectionGrain().GetUsersProjects(user, operation);
            if (result is null) {
                return this.Forbid();
            }
            return result.ToListProject();
        }
    }

    // GET api/Me
    [HttpGet("ToDo", Name = "MeTodoGetAll")]
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
            var result = await this.Client.GetUserToDoGrain(user.UserId).GetUsersToDos(operation);
            return result.ToListToDo();
        }
    }
}
