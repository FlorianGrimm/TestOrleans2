using System;
using System.Collections.Generic;

namespace TestOrleans2.OData
{
    public partial class ToDoHistory
    {
        public Guid OperationId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ToDoId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public long SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
    }
}
