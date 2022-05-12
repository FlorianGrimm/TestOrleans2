using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TestOrleans2.OData;

namespace TestOrleans2.OData.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoesController : ControllerBase {
        private readonly TodoDBContext _context;

        public ToDoesController(TodoDBContext context) {
            this._context = context;
        }

        // GET: api/ToDoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDo() {
            return await this._context.ToDo.ToListAsync();
        }

        // GET: api/ToDoes/5
        [HttpGet("{toDoId}")]
        public async Task<ActionResult<ToDo>> GetToDo(Guid toDoId) {
            var toDo = await this._context.ToDo.FindAsync(toDoId);

            if (toDo is null) {
                return this.NotFound();
            } else {
                return toDo;
            }
        }

        // PUT: api/ToDoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{toDoId}")]
        public async Task<IActionResult> PutToDo(Guid toDoId, ToDo toDo) {
            if (toDoId != toDo.ToDoId) {
                return this.BadRequest();
            }

            this._context.Entry(toDo).State = EntityState.Modified;

            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!this.ToDoExists(toDoId)) {
                    return this.NotFound();
                } else {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/ToDoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDo(ToDo toDo) {
            this._context.ToDo.Add(toDo);
            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (ToDoExists(toDo.ToDoId)) {
                    return this.Conflict();
                } else {
                    throw;
                }
            }

            return this.CreatedAtAction("GetToDo", new { toDoId = toDo.ToDoId }, toDo);
        }

        // DELETE: api/ToDoes/5
        [HttpDelete("{toDoId}")]
        public async Task<IActionResult> DeleteToDo(Guid toDoId) {
            var toDo = await this._context.ToDo.FindAsync(toDoId);
            if (toDo == null) {
                return this.NotFound();
            }

            this._context.ToDo.Remove(toDo);
            await this._context.SaveChangesAsync();

            return this.NoContent();
        }
        
        private bool ToDoExists(Guid id) {
            return this._context.ToDo.Any(e => e.ToDoId == id);
        }
    }
}
