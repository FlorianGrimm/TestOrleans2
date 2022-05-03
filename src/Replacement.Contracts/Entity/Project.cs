﻿namespace Replacement.Contracts.Entity;
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


public record class ProjectEntity(
    // [property: Key]
    Guid ProjectId,
    // [property: StringLength(50)]
    string Title,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long SerialVersion
) : IDataOperationRelated {
    public ProjectPK GetPrimaryKey() => new ProjectPK(this.ProjectId);
    public OperationPK GetOperationPK() => new OperationPK(this.ModifiedAt, this.OperationId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public ProjectEntity SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.SerialVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.SerialVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }
}

partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static ProjectAPI? ToProjectAPI(this ProjectEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectAPI(
                ProjectId: that.ProjectId,
                Title: that.Title,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<ProjectAPI> ToListProjectAPI(
        this IEnumerable<ProjectEntity> that) {
        var result = new List<ProjectAPI>();
        foreach (var e in that) {
            if (e is not null) {
                var a = e.ToProjectAPI();
                result.Add(a);
            }
        }
        return result;
    }
}

public record class ProjectSelectPKResult(
    List<ProjectEntity> Projects,
    List<ToDoEntity> ToDos
    );