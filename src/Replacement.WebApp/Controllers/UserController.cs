namespace Replacement.WebApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ReplacementControllerBase {
    public UserController(
        IClusterClient client,
        ILogger<UserController> logger
        )
        : base(client, logger) {
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserAPI>>> Get() {
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
            var grain = this.Client.GetGrain<IUserCollectionGrain>(Guid.Empty)!;
            var result = await grain.GetAllUsers(operation);
            return result.ToListUserAPI();
        }
    }

    // GET api/User/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserAPI?>> Get(Guid userId) {
        var requestOperation = this.CreateRequestOperation(
                    pk: userId,
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
            var grain = this.Client.GetUserGrain(userId);
            var result = await grain.GetUser(operation);
            return result.ToUserAPI();
        }
    }

    // POST api/User
    [HttpPost]
    public async Task<ActionResult<UserAPI>> Post([FromBody] UserAPI value) {
        if (value.UserId == Guid.Empty) {
            value = value with {
                UserId = Guid.NewGuid()
            };
        }

        var requestOperation = this.CreateRequestOperation(
            pk: value.UserId,
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
            var grain = this.Client.GetUserGrain(value.UserId);
            var result = await grain.UpsertUser(value.ToUserEntity(), user, operation);
            if (result is not null) {
                return result.ToUserAPI();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/User/5
    [HttpPut("{userId}")]
    public async Task<ActionResult<UserAPI>> Put(Guid userId, [FromBody] UserAPI value) {
        value = value with { UserId = userId };

        var requestOperation = this.CreateRequestOperation(
            pk: value.UserId,
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
            var grain = this.Client.GetGrain<IUserGrain>(value.UserId);
            var result = await grain.UpsertUser(value.ToUserEntity(), user, operation);
            if (result is not null) {
                return result.ToUserAPI();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/User/5
    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete(Guid userId) {
        if (userId == Guid.Empty) {
            return this.NotFound();
        }

        var requestOperation = this.CreateRequestOperation(
            pk: userId,
            argument: (UserAPI?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: true,
            createUserIfNeeded: true);

        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetGrain<IUserGrain>(userId).DeleteUser(user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.NotFound();
            }
        }
    }
}
