using System;
using System.Collections.Generic;

namespace TestOrleans2.OData
{
    public partial class Project
    {
        public Project()
        {
            this.ToDo = new HashSet<ToDo>();
        }

        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public long SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
}
