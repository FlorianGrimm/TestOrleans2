using System;
using System.Collections.Generic;

namespace TestOrleans2.OData
{
    public partial class User
    {
        public User()
        {
            this.ToDo = new HashSet<ToDo>();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
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
