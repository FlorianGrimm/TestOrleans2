using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class ToDo
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public byte[] SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
