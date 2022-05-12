/*
 import-module ".\bin\Debug\net6.0\TestOrleans2.Powershell.dll"
 */



namespace TestOrleans2.Powershell;

[Cmdlet(VerbsCommon.New, "ReplacementConnection")]
[OutputType(typeof(ReplacementConnection))]
public class NewReplacementConnectionCommand : PSCmdlet {
    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string Url { get; set; }

    //[Parameter(
    //    Position = 1,
    //    ValueFromPipelineByPropertyName = true)]
    //[ValidateSet("Cat", "Dog", "Horse")]
    //public string FavoritePet { get; set; } = "Dog";

    public NewReplacementConnectionCommand() {
        this.Url = string.Empty;
    }

    // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
    protected override void ProcessRecord() {
        var result = new ReplacementConnection {
            Url = Url
        };
        ReplacementConnection.Current = result;
        WriteObject(result);
    }
}

public class ReplacementConnection {
    public static ReplacementConnection? Current;
    private ReplacementClient? _Client;

    public ReplacementConnection() {
        this.Url = string.Empty;
        this.UseDefaultCredentials = true;
    }

    public string Url { get; set; }
    public bool UseDefaultCredentials { get; set; }

    public TestOrleans2.Client.ReplacementClient GetClient(bool create) {
        if (!create && (this._Client is not null)) {
            return this._Client;
        }
        var handler = new System.Net.Http.HttpClientHandler();
        if (this.UseDefaultCredentials) {
            handler.UseDefaultCredentials = true;
        }
        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient(
            handler
            );
        var result = new TestOrleans2.Client.ReplacementClient(this.Url, httpClient);
        this._Client = result;
        return result;
    }
}
