namespace Replacement.TestExtensions;


[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

    // from IAsyncLifetime
    public virtual async Task InitializeAsync() {
        await this.InitializeTestClusterAsync();
        await this.InitializeTestWebServerAsync();
    }

    public virtual async Task InitializeTestClusterAsync() {
        var builder = new TestClusterBuilder(initialSilosCount: 1);
        this.ConfigureTestClusterBuilder(builder);
        var testCluster = builder.Build();
        if (testCluster.Primary == null) {
            await testCluster.DeployAsync().ConfigureAwait(false);
        }
        this.HostedCluster = testCluster;
        this.Logger = testCluster.Client.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Application");
    }

    public virtual void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
    }

    public virtual async Task InitializeTestWebServerAsync() {
        string[] args = new string[]{
            "--ASPNETCORE_ENVIRONMENT=UnitTest",
            "--environment=UnitTest"
        };
        var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);

        this.ConfigureTestWebAppHost(hostBuilder);
        var host = hostBuilder.Build();
        this._WebTestHost = host;
        this._WebTestServer = host.GetTestServer();
        await host.StartAsync();
    }

    public virtual void ConfigureTestWebAppHost(IHostBuilder hostBuilder) {
        hostBuilder.ConfigureAppConfiguration((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) => {
            configurationBuilder.AddUserSecrets(this.GetType().Assembly);
        });
        hostBuilder.ConfigureServices((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
            //hostBuilderContext.
            services.AddSingleton<IClusterClient>((sp) => {
                return this.ClusterClient ?? throw new ObjectDisposedException(nameof(this.ClusterClient));
            });

            services.AddAuthentication(TestAuthenticationDefaults.AuthenticationScheme)
                .AddTestAuthentication((options) => {
                    options.AddTestUser(@"unittest\otto");
                });
        });
        hostBuilder.ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseTestServer();
            webBuilder.UseStartup<Replacement.WebApp.Startup>();
        });
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

        if (this._WebTestHost is not null) {
            var host = this._WebTestHost;
            this._WebTestHost = null;
            await host.StopAsync();
            host.Dispose();
        }
    }

    public System.Net.Http.HttpClient CreateWebClient() => this.WebTestServer.CreateClient();

    public Replacement.Client.ReplacementClient CreateReplacementClient(string? username = default) {
        TestServer webTestServer = this.WebTestServer;
        var client = webTestServer.CreateClient();
        var baseAddress = webTestServer.BaseAddress!;
        var replacementClient = new Replacement.Client.ReplacementClient(baseAddress.ToString(), client);
        if (!string.IsNullOrEmpty(username)) {
            replacementClient.OnGetAuthorizationHeader = (_, _, _) => {
                return TestAuthenticationHandlerExtensions.CreateAuthorizationHeader(@"unittest\otto");
            };
        }
        return replacementClient;
    }
}
