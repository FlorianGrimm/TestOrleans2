using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class Operation
    {
        public Operation()
        {
            Project = new HashSet<Project>();
            ProjectHistory = new HashSet<ProjectHistory>();
            ToDo = new HashSet<ToDo>();
            ToDoHistory = new HashSet<ToDoHistory>();
            User = new HashSet<User>();
            UserHistory = new HashSet<UserHistory>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Data { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public byte[] SerialVersion { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectHistory> ProjectHistory { get; set; }
        public virtual ICollection<ToDo> ToDo { get; set; }
        public virtual ICollection<ToDoHistory> ToDoHistory { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}
