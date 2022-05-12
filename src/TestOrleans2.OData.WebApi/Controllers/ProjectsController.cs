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
    public class ProjectsController : ControllerBase {
        private readonly TodoDBContext _context;

        public ProjectsController(TodoDBContext context) {
            this._context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject() {
            return await this._context.Project.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProject(Guid projectId) {
            var project = await this._context.Project.FindAsync(projectId);

            if (project == null) {
                return this.NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{projectId}")]
        public async Task<IActionResult> PutProject(Guid projectId, Project project) {
            if (projectId != project.ProjectId) {
                return this.BadRequest();
            }

            this._context.Entry(project).State = EntityState.Modified;

            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if ((await this._context.Project.FindAsync(projectId)) is null) {
                    return this.NotFound();
                } else {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project) {
            this._context.Project.Add(project);
            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (ProjectExists(project.ProjectId)) {
                    return this.Conflict();
                } else {
                    throw;
                }
            }

            return this.CreatedAtAction("GetProject", new { projectId = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid id) {
            var project = await this._context.Project.FindAsync(id);
            if (project == null) {
                return this.NotFound();
            }

            this._context.Project.Remove(project);
            await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProjectExists(Guid projectId) {
            return this._context.Project.Any(e => e.ProjectId == projectId);
        }
    }
}
