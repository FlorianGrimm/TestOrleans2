namespace Replacement.TestExtensions;

public class BaseClusterFixture : Xunit.IAsyncLifetime {
    public TestServer WebTestServer => _WebTestServer ?? throw new ObjectDisposedException("WebTestServer");

    private IHost? _WebTestHost;
    private TestServer? _WebTestServer;

    public TestCluster? HostedCluster { get; private set; }


    public IGrainFactory? GrainFactory => this.HostedCluster?.GrainFactory;

    public IClusterClient? ClusterClient => this.HostedCluster?.Client;

    public ILogger? Logger { get; private set; }

    public ITestOutputHelper? TestOutputHelper;

    public BaseClusterFixture() {
    }

    public virtual void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
    }

    // from IAsyncLifetime
    public virtual async Task InitializeAsync() {
        var builder = new TestClusterBuilder();
        this.ConfigureTestClusterBuilder(builder);
        var testCluster = builder.Build();
        if (testCluster.Primary == null) {
            await testCluster.DeployAsync().ConfigureAwait(false);
        }
        this.HostedCluster = testCluster;
        this.Logger = testCluster.Client.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Application");

        /*
        var applicationServices = testCluster.ClusterClient.ServiceProvider;
        var featureCollection = new Microsoft.AspNetCore.Http.Features.FeatureCollection();
        TestServerOptions testServerOptions = new TestServerOptions();
        this._WebTestServer = new TestServer(applicationServices, featureCollection, Options.Create(testServerOptions));
        this.HttpApplication = TestApplication.Create(this._WebTestServer, applicationServices, (Microsoft.AspNetCore.Builder.IApplicationBuilder applicationBuilder) => {
            var startup = applicationServices.GetRequiredService<Replacement.WebApp.Startup>();
            var env = applicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
            startup.Configure(applicationBuilder, env);
        });
        await ((Microsoft.AspNetCore.Hosting.Server.IServer)this._WebTestServer).StartAsync(this.HttpApplication, System.Threading.CancellationToken.None);
        */
        //ASPNETCORE_
        //Hosting:Environment
        string[] args=new string[]{
            "--ASPNETCORE_ENVIRONMENT=UnitTest"
        };
        var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
        hostBuilder.ConfigureServices((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
            //hostBuilderContext.
            services.AddSingleton<IClusterClient>((sp) => {
                return this.ClusterClient ?? throw new ObjectDisposedException(nameof(this.ClusterClient));
            });
        });
        hostBuilder.ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseTestServer();
            webBuilder.UseStartup<Replacement.WebApp.Startup>();
        });
        var host = hostBuilder.Build();
        this._WebTestHost = host;
        this._WebTestServer = host.GetTestServer();
        await host.StartAsync();
    }

    // from IAsyncLifetime
    public virtual async Task DisposeAsync() {
        var cluster = this.HostedCluster;
        if (cluster is null) return;

        try {
            await cluster.StopAllSilosAsync().ConfigureAwait(false);
        } finally {
            await cluster.DisposeAsync().ConfigureAwait(false);
        }

        //if (this._WebTestServer is Microsoft.AspNetCore.Hosting.Server.IServer iserver) {
        //    await iserver.StopAsync(System.Threading.CancellationToken.None);
        //    this._WebTestServer = null;
        //}
        if (this._WebTestHost is not null) {
            var host = this._WebTestHost;
            this._WebTestHost = null;
            await host.StopAsync();
            host.Dispose();
        }
    }

    public System.Net.Http.HttpClient CreateWebClient() => this.WebTestServer.CreateClient();

    public Replacement.Client.ReplacementClient CreateReplacementClient() {
        TestServer webTestServer = this.WebTestServer;
        var client = webTestServer.CreateClient();
        var baseAddress = webTestServer.BaseAddress!;
        var replacementClient = new Replacement.Client.ReplacementClient(baseAddress.ToString(), client);
        return replacementClient;
    }
}
