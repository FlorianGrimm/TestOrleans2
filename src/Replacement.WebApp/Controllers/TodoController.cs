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
        var operation = new Replacement.Contracts.API.Operation(
               Guid.NewGuid(),
               this.GetOperationTitle(),
               nameof(ToDo),
               "",
               this.GetOperationData(),
               DateTimeOffset.Now,
               0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            return await this.Client.GetToDoCollectionGrain().GetAllToDos(user, operation);
        }
    }

    // GET api/ToDo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{todoId}")]
    public async Task<ActionResult<ToDo?>> Get(Guid todoId) {
        var operation = new Replacement.Contracts.API.Operation(
                Guid.NewGuid(),
                this.GetOperationTitle(),
                nameof(ToDo),
                todoId.ToString(),
                this.GetOperationData(),
                DateTimeOffset.Now,
                0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetToDoGrain(todoId);
            var result = await grain.GetToDo(user, operation);
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
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(ToDo),
            value.ToDoId.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetToDoGrain(value.ToDoId);
            var result = await grain.UpsertToDo(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/ToDo/5
    [HttpPut("{toDoId}")]
    public async Task<ActionResult> Put(Guid toDoId, [FromBody] ToDo value) {
        value = value with { ToDoId = toDoId };
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(ToDo),
            toDoId.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IToDoGrain>(value.ToDoId);
            var result = await grain.UpsertToDo(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/ToDo/5
    [HttpDelete("{todoId}")]
    public async Task<ActionResult> Delete(Guid todoId) {
        if (todoId == Guid.Empty) {
            return NotFound();
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(ToDo),
            todoId.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        (operation, User? user) = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetToDoGrain(todoId).DeleteToDo(user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
