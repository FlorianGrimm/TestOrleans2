namespace Replacement.WebApp.Controllers;
//https://localhost:5001/api/ToDo
//https://localhost:5001/api/ToDo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
[Route("api/[controller]")]
[ApiController]
public class ToDoController : ReplacementControllerBase {
    public ToDoController(IClusterClient client, ILogger logger)
        : base(client, logger) {
    }

    // GET: api/ToDo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDo>>> Get() {
        var operation = new Operation(
               OperationId: Guid.NewGuid(),
               Title: this.GetOperationTitle(),
               EntityType: nameof(ToDo),
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
            return await this.Client.GetToDoCollectionGrain().GetAllToDos(user, operation);
        }
    }

    // GET api/ToDo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{projectId}/{todoId}")]
    public async Task<ActionResult<ToDo?>> Get(Guid projectId, Guid toDoId) {
        var toDoPK = new ToDoPK(projectId, toDoId);
        var operation = new Operation(
                OperationId: Guid.NewGuid(),
                Title: this.GetOperationTitle(),
                EntityType: nameof(ToDo),
                EntityId: toDoPK.ToString(),
                Data: this.GetOperationData(toDoId),
                UserId: null,
                CreatedAt: DateTimeOffset.Now,
                SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(toDoPK.ProjectId);
            var result = await grain.GetToDo(toDoPK, user, operation);
            return result;
        }
    }

    // POST api/ToDo
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ToDo value) {
        if (value.ToDoId == Guid.Empty) {
            value = value with {
                ToDoId = Guid.NewGuid()
            };
        }
        var toDoPK = value.GetPrimaryKey();
        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(ToDo),
            EntityId: toDoPK.ToString(),
            Data: this.GetOperationData(value),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetToDoGrain(toDoPK);
            var result = await grain.UpsertToDo(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/ToDo/5
    [HttpPut("{projectId}/{toDoId}")]
    public async Task<ActionResult> Put(Guid projectId, Guid toDoId, [FromBody] ToDo value) {
        value = value with {
            ProjectId = projectId,
            ToDoId = toDoId
        };
        var toDoPK = value.GetPrimaryKey();

        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(ToDo),
            EntityId: toDoPK.ToString(),
            Data: this.GetOperationData(value),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetProjectGrain(toDoPK.ProjectId);
            var toDo = await grain.UpsertToDo(value, user, operation);
            if (toDo is not null) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/ToDo/5
    [HttpDelete("{projectId}/{todoId}")]
    public async Task<ActionResult> Delete(Guid projectId, Guid todoId) {
        if ((projectId == Guid.Empty) || (todoId == Guid.Empty)) {
            return NotFound();
        }
        var toDoPK = new ToDoPK(projectId, todoId);

        var operation = new Operation(
            OperationId: Guid.NewGuid(),
            Title: this.GetOperationTitle(),
            EntityType: nameof(ToDo),
            EntityId: toDoPK.ToString(),
            Data: this.GetOperationData(toDoPK),
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetProjectGrain(todoId).DeleteToDo(toDoPK, user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
