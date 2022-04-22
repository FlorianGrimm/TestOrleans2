// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Replacement.Contracts.API;
using Replacement.Repository.Grains;

namespace Replacement.WebApp.Controllers;

//https://localhost:5001/api/Todo
//https://localhost:5001/api/Todo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase {
    private readonly IClusterClient _Client;

    public TodoController(IClusterClient client) {
        this._Client = client;
    }
    // GET: api/Todo
    [HttpGet]
    public  async Task<ActionResult<IEnumerable<ToDo>>> Get() {
        var grain = this._Client.GetGrain<ITodoCollectionGrain>(Guid.Empty)!;
        return await grain.GetAllTodos(this.HttpContext.User);
    }

    // GET api/Todo/9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDo?>> Get(Guid id) {
        var grain = this._Client.GetGrain<ITodoGrain>(id);
        return await grain.GetTodo(this.HttpContext.User);
    }

    // POST api/Todo
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ToDo value) {
        if (value.Id == Guid.Empty) {
            value = value with {
                Id = Guid.NewGuid()
            };
        }
        var grain = this._Client.GetGrain<ITodoGrain>(value.Id);
        var result = await grain.UpsertTodo(value, this.HttpContext.User);
        if (result) {
            return this.Ok();
        } else {
            return this.Conflict();
        }
    }

    // PUT api/Todo/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] ToDo value) {
        value = value with { Id = id };
        var grain = this._Client.GetGrain<ITodoGrain>(value.Id);
        var result = await grain.UpsertTodo(value, this.HttpContext.User);
        if (result) {
            return this.Ok();
        } else {
            return this.Conflict();
        }
    }

    // DELETE api/Todo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id) {
        
        if (id == Guid.Empty) {
            return NotFound();
        } else {
            var value = new ToDo(id, null, null, "", false, null, Contracts.Consts.MinDateTimeOffset, Contracts.Consts.MaxDateTimeOffset, 0);
            var result = await this._Client.GetGrain<ITodoGrain>(id).DeleteTodo(value, this.HttpContext.User);
            if (result) {
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}

