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
    public async Task<ActionResult<IEnumerable<User>>> Get() {
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
            var grain = this.Client.GetGrain<IUserCollectionGrain>(Guid.Empty)!;
            return await grain.GetAllUsers(operation);
        }
    }

    // GET api/User/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{userId}")]
    public async Task<ActionResult<User?>> Get(Guid userId) {
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(User),
                EntityId: userId.ToString(),
                Data: this.GetOperationData(userId),
                UserId: null,
                CreatedAt: DateTimeOffset.Now,
                SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetUserGrain(userId);
            var result = await grain.GetUser(operation);
            return result;
        }
    }

    // POST api/User
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User value) {
        if (value.UserId == Guid.Empty) {
            value = value with {
                UserId = Guid.NewGuid()
            };
        }
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(User),
            EntityId: value.UserId.ToString(),
            Data: this.GetOperationData(value),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetUserGrain(value.UserId);
            var result = await grain.UpsertUser(value, user, operation);
            if (result is not null) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/User/5
    [HttpPut("{userId}")]
    public async Task<ActionResult> Put(Guid userId, [FromBody] User value) {
        value = value with { UserId = userId };
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(User),
            EntityId: userId.ToString(),
            Data: this.GetOperationData(value),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IUserGrain>(value.UserId);
            var result = await grain.UpsertUser(value, user, operation);
            if (result is not null) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/User/5
    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete(Guid userId) {
        if (userId == Guid.Empty) {
            return NotFound();
        }
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(User),
            EntityId: userId.ToString(),
            Data: this.GetOperationData(userId),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetGrain<IUserGrain>(userId).DeleteUser(user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
