﻿namespace Replacement.WebApp.Controllers;
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
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IToDoCollectionGrain>(Guid.Empty)!;
            return await grain.GetAllToDos(user, operation);
        }
    }

    // GET api/ToDo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDo?>> Get(Guid id) {
        var operation = new Replacement.Contracts.API.Operation(
                Guid.NewGuid(),
                this.GetOperationTitle(),
                nameof(ToDo),
                id.ToString(),
                this.GetOperationData(),
                DateTimeOffset.Now,
                0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetToDoGrain(id);
            var result = await grain.GetToDo(user, operation);
            return result;
        }
    }

    // POST api/ToDo
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
            nameof(ToDo),
            value.Id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetToDoGrain(value.Id);
            var result = await grain.UpsertToDo(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // PUT api/ToDo/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] ToDo value) {
        value = value with { Id = id };
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(ToDo),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var grain = this.Client.GetGrain<IToDoGrain>(value.Id);
            var result = await grain.UpsertToDo(value, user, operation);
            if (result) {
                return this.Ok();
            } else {
                return this.Conflict();
            }
        }
    }

    // DELETE api/ToDo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id) {
        if (id == Guid.Empty) {
            return NotFound();
        }
        var operation = new Replacement.Contracts.API.Operation(
            Guid.NewGuid(),
            this.GetOperationTitle(),
            nameof(ToDo),
            id.ToString(),
            this.GetOperationData(),
            DateTimeOffset.Now,
            0);
        var user = await this.GetUserByUserName(operation);
        if (user is null) {
            return this.Forbid();
        }
        {
            var value = new ToDo(id, null, null, "", false, null, Contracts.Consts.MinDateTimeOffset, Contracts.Consts.MaxDateTimeOffset, 0);
            var result = await this.Client.GetGrain<IToDoGrain>(id).DeleteToDo(value, user, operation);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
