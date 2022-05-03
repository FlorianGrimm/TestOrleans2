namespace Replacement.Contracts.Entity;
/*
public partial class Operation {
    public Operation() {
        this.Project = new HashSet<Project>();
        this.ProjectHistory = new HashSet<ProjectHistory>();
        this.ToDo = new HashSet<ToDo>();
        this.ToDoHistory = new HashSet<ToDoHistory>();
        this.User = new HashSet<User>();
        this.UserHistory = new HashSet<UserHistory>();
    }

    [Key]
    public Guid Id { get; set; }
    [StringLength(100)]
    public string Title { get; set; } = null!;
    [StringLength(100)]
    public string EntityType { get; set; } = null!;
    [StringLength(100)]
    public string EntityId { get; set; } = null!;
    public string? Data { get; set; }
    [Key]
    public DateTimeOffset CreatedAt { get; set; }
    public long SerialVersion { get; set; }

    [InverseProperty("Operation")]
    public virtual ICollection<Project> Project { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ProjectHistory> ProjectHistory { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ToDo> ToDo { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ToDoHistory> ToDoHistory { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<User> User { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<UserHistory> UserHistory { get; set; }
}
*/
/*
CREATE TABLE [dbo].[Operation] (
    [OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [OperationName] VARCHAR (100)      NOT NULL,
    [EntityType]    VARCHAR (50)       NOT NULL,
    [EntityId]      NVARCHAR (100)     NOT NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_Operation] PRIMARY KEY CLUSTERED ([CreatedAt] ASC,[OperationId] ASC)
);
*/
public record class Operation(
    Guid OperationId,
    // [property:StringLength(100)]
    string OperationName,
    // [property: StringLength(50)]
    string EntityType,
    // [property: StringLength(100)]
    string EntityId,
    Guid? UserId,
    // [property: Key]
    DateTimeOffset CreatedAt,
    long SerialVersion
);


/*
 CREATE TABLE [dbo].[Request]
(
	[RequestId]     UNIQUEIDENTIFIER   NOT NULL,
	[OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [ActivityId]    VARCHAR (200)      NOT NULL,
    [OperationName] VARCHAR (100)      NOT NULL,
    [EntityType]    VARCHAR (50)       NOT NULL,
    [EntityId]      NVARCHAR (100)     NOT NULL,
    [Argument]      NVARCHAR (MAX)     NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_Request] PRIMARY KEY CLUSTERED ([RequestId] ASC)
)

*/

public record class RequestLog(
    Guid RequestLogId,
    Guid OperationId,
    // [property:StringLength(200)]
    string ActivityId,
    // [property:StringLength(100)]
    string OperationName,
    // [property: StringLength(50)]
    string EntityType,
    // [property: StringLength(100)]
    string EntityId,
    string? Argument,
    Guid? UserId,
    // [property: Key]
    DateTimeOffset CreatedAt,
    long SerialVersion
);

public record class RequestOperation(
    Guid RequestLogId,
    Guid OperationId,
    // [property:StringLength(200)]
    string ActivityId,
    // [property:StringLength(100)]
    string OperationName,
    // [property: StringLength(50)]
    string EntityType,
    // [property: StringLength(100)]
    string EntityId,
    string? Argument,
    string? UserName,
    Guid? UserId,
    // [property: Key]
    DateTimeOffset CreatedAt,
    long SerialVersion
);
