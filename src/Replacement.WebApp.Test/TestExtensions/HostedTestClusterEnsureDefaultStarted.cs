using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replacement.TestExtensions;
/// <summary>
/// Base class that ensures a silo cluster is started with the default configuration, and avoids restarting it if the previous test used the same default base.
/// </summary>
[Collection("DefaultCluster")]
public abstract class HostedTestClusterEnsureDefaultStarted : OrleansTestingBase {
    protected DefaultClusterFixture Fixture { get; private set; }
    protected TestCluster HostedCluster => this.Fixture.HostedCluster ?? throw new InvalidOperationException("this.Fixture.HostedCluster is null");

    protected IGrainFactory GrainFactory => this.HostedCluster.GrainFactory;

    protected IClusterClient Client => this.HostedCluster.Client;

    protected ILogger Logger => this.Client.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(this.GetType().Namespace ?? "Application");

    protected HostedTestClusterEnsureDefaultStarted(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) {
        fixture.TestOutputHelper = output;
        this.Fixture = fixture;
        //this.loggerFactory = new LoggerFactory(new[] { new XunitLoggerProvider(this.output) });
    }
}
