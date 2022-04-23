
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace Replacement.Contracts.API;
/*
    public partial class Project {
        public Project() {
            this.ToDo = new HashSet<ToDo>();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ModifiedAt,OperationId")]
        [InverseProperty("Project")]
        public virtual Operation? Operation { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
*/


public record class Project(
    [property: Key]
    Guid Id,
    [property: StringLength(50)]
    string Title,
    Guid? OperationId,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    long SerialVersion
);