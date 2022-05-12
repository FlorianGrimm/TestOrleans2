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
    public class UsersController : ControllerBase {
        private readonly TodoDBContext _context;

        public UsersController(TodoDBContext context) {
            this._context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser() {
            return await this._context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(Guid userId) {
            var user = await this._context.User.FindAsync(userId);

            if (user == null) {
                return this.NotFound();
            } else {
                return user;
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(Guid userId, User user) {
            if (userId != user.UserId) {
                return this.BadRequest();
            }

            this._context.Entry(user).State = EntityState.Modified;

            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!this.UserExists(userId)) {
                    return this.NotFound();
                } else {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user) {
            this._context.User.Add(user);
            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (UserExists(user.UserId)) {
                    return this.Conflict();
                } else {
                    throw;
                }
            }

            return this.CreatedAtAction("GetUser", new { userId = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId) {
            var user = await this._context.User.FindAsync(userId);
            if (user is null) {
                return this.NotFound();
            } else {
                this._context.User.Remove(user);
                await this._context.SaveChangesAsync();
                return this.NoContent();
            }
        }

        private bool UserExists(Guid userId) {
            return this._context.User.Any(e => e.UserId == userId);
        }
    }
}
