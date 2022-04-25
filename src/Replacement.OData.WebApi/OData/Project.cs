using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class Project
    {
        public Project()
        {
            ToDo = new HashSet<ToDo>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public byte[] SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
}
