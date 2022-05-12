using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class ProjectHistory
    {
        public Guid OperationId { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
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
