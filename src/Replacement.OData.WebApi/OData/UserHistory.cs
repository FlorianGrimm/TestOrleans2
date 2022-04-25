using System;
using System.Collections.Generic;

namespace Replacement.OData
{
    public partial class UserHistory
    {
        public Guid OperationId { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public byte[] SerialVersion { get; set; }

        public virtual Operation Operation { get; set; }
    }
}
