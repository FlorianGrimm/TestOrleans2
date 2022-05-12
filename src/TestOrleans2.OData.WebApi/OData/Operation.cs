using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class Operation
    {
        public Operation()
        {
            this.Project = new HashSet<Project>();
            this.ProjectHistory = new HashSet<ProjectHistory>();
            this.ToDo = new HashSet<ToDo>();
            this.ToDoHistory = new HashSet<ToDoHistory>();
            this.User = new HashSet<User>();
            this.UserHistory = new HashSet<UserHistory>();
        }

        public Guid OperationId { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Data { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public long SerialVersion { get; set; }
        

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectHistory> ProjectHistory { get; set; }
        public virtual ICollection<ToDo> ToDo { get; set; }
        public virtual ICollection<ToDoHistory> ToDoHistory { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}
