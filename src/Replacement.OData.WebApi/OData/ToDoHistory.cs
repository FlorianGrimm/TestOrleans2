using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class ToDoHistory
    {
        public Guid OperationId { get; set; }
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public byte[] SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
    }
}
