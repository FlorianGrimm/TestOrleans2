namespace TestOrleans2.WebApp.Controllers;

public class ReplacementControllerBase : ControllerBase {
    public readonly IClusterClient Client;
    public readonly ILogger Logger;

    protected ReplacementControllerBase(
        IClusterClient client,
        ILogger logger
        ) {
        this.Client = client;
        this.Logger = logger;
    }
}

