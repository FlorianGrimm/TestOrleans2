namespace TestOrleans2.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosticsController : ReplacementControllerBase {
    public DiagnosticsController(
        IClusterClient client,
        ILogger<UserController> logger
        )
        : base(client, logger) {
    }

    // GET api/Diagnostics
    [HttpGet("RequestLog", Name = "DiagnosticsRequestLogFiltered")]
    public async Task<ActionResult<List<RequestLog>>> GetRequestLogFiltered(RequestLogFilter filter) {
        var requestOperation = this.CreateRequestOperation(
            pk: string.Empty,
            argument: (RequestLog?)null
            );
        var (operation, user) = await this.InitializeOperation(
            requestOperation: requestOperation,
            canModifyState: false,
            createUserIfNeeded: false);
        if (user is null) {
            return this.Forbid();
        }
        {
            var result = await this.Client.GetRequestLogCollectionGrain().GetAllRequestLogs(filter, operation);
            return result.ToListRequestLog();
        }
    }
}