namespace Replacement.Powershell;

[Cmdlet(VerbsCommon.Get, "RequestLog")]
[OutputType(typeof(RequestLog[]))]
public class GetRequestLogCommand : PSCmdlet {
    [Parameter(
         Mandatory = false,
         Position = 0,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public ReplacementConnection? Connection { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 1,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public Guid? RequestLogId { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 1,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public Guid? OperationId { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 2,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public string? ActivityId { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 3,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public string? OperationName { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 4,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public string? EntityType { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 5,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public string? EntityId { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 6,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public string? Argument { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 7,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public Guid? UserId { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 8,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public DateTimeOffset? CreatedAtLow { get; set; }

    [Parameter(
         Mandatory = false,
         Position = 9,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
    public DateTimeOffset? CreatedAtHigh { get; set; }

    public GetRequestLogCommand() : base() {
    }

    // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
    protected override void BeginProcessing() {
        if (this.Connection is null) {
            this.Connection = ReplacementConnection.Current;
        }
        if (this.Connection is null) {
            WriteError(new ErrorRecord(new InvalidOperationException("Connection is needed"), "", ErrorCategory.InvalidArgument, null));
        }
    }

    // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
    protected override void ProcessRecord() {
        var client = this.Connection!.GetClient(false);
        RequestLogFilter filter = new RequestLogFilter(
            RequestLogId:this.RequestLogId,
            OperationId:this.OperationId,
            ActivityId:this.ActivityId,
            OperationName:this.OperationName,
            EntityType:this.EntityType,
            EntityId:this.EntityId,
            Argument:this.Argument,
            UserId:this.UserId,
            CreatedAtLow:this.CreatedAtLow,
            CreatedAtHigh:this.CreatedAtHigh
            );
        var result = client!.DiagnosticsRequestLogFilteredAsync(filter).GetAwaiter().GetResult();
        if (result is not null) { 
            this.WriteObject(result);
        }
    }

    // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
    protected override void EndProcessing() {
    }
}
