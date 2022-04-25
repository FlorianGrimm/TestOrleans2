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
        var operation = new Replacement.Contracts.API.Operation(
               Guid.NewGuid(),
               this.GetOperationTitle(),
               nameof(User),
               "",
               this.GetOperationData(),
               DateTimeOffset.Now,
               0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IUserCollectionGrain>(Guid.Empty)!;
            return await grain.GetAllUsers(user, operation);
        }
    }

    // GET api/User/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> Get(Guid id) {
        var operation = new Replacement.Contracts.API.Operation(
                Guid.NewGuid(),
                this.GetOperationTitle(),
                nameof(User),
                id.ToString(),
                this.GetOperationData(),
                DateTimeOffset.Now,
                0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetUserGrain(id);
            var result = await grain.GetUser(user, operation);
            return result;
        }
    }

    // POST api/User
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User value) {
        if (value.Id == Guid.Empty) {
            value = value with {
                Id = Guid.NewGuid()
            };
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(User),
            value.Id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetUserGrain(value.Id);
            var result = await grain.UpsertUser(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/User/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] User value) {
        value = value with { Id = id };
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(User),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IUserGrain>(value.Id);
            var result = await grain.UpsertUser(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/User/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id) {
        if (id == Guid.Empty) {
            return NotFound();
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(User),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var value = new User(id, null, null, "", false, null, Contracts.Consts.MinDateTimeOffset, Contracts.Consts.MaxDateTimeOffset, 0);
            var result = await this.Client.GetGrain<IUserGrain>(id).DeleteUser(value, user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
