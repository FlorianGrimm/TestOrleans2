using System.Net.Http;

namespace Replacement.Console;

public static class Program {
    private static int cntProjectRead = 0;
    private static int cntProjectWrite = 0;
    private static int cntProjectUpdate = 0;
    private static TimeSpan tsTotal=TimeSpan.Zero;

#if true
    private static int cntThreads = 10;
    private static int cntOuter = 200;
    private static int cntInnerWrite = 10;
    private static int cntInnerRead = 40;
    private static int cntInnerUpdate = 10;
#else
    private static int cntThreads = 1;
    private static int cntOuter = 1;
    private static int cntInnerWrite = 1;
    private static int cntInnerRead = 1;
    private static int cntInnerUpdate = 1;
#endif

    public static async Task<int> Main(string[] args) {
        System.Console.Out.WriteLine("Replacement.Console!");
        var configuration = ConfigureApp(args);
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        ConfigureService(configuration, services);
        var appServiceProvider = services.BuildServiceProvider();

        int result;
        try {
            var dtStart = System.DateTime.UtcNow;
            System.Console.Out.WriteLine($"dtStart : {dtStart}");
#if true
            var tasks = System.Linq.Enumerable.Range(0, cntThreads).Select(
                _ => Run(appServiceProvider)
                ).ToArray();
            var results = await Task.WhenAll(tasks);
            result = results.Max();
#else
            result = await Run(appServiceProvider);
#endif

            var dtEnd = System.DateTime.UtcNow;
            var tsDuration = dtEnd.Subtract(dtStart);
            System.Console.Out.WriteLine($"duration total: {tsDuration.TotalSeconds} sec");

            System.Console.Out.WriteLine($"cntProjectRead : {cntProjectRead}");
            System.Console.Out.WriteLine($"cntProjectWrite: {cntProjectWrite}");
            System.Console.Out.WriteLine($"cntProjectUpdate: {cntProjectUpdate}");

            var durationPerRequest = tsDuration.TotalMilliseconds / (cntProjectRead + cntProjectWrite + cntProjectUpdate);
            System.Console.Out.WriteLine($"duration per request: {durationPerRequest} ms");

        } catch (System.Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            result = 1;
        }

        return result;
    }

    private static IConfigurationRoot ConfigureApp(string[] args) {
        var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        builder.AddCommandLine(args);
        builder.AddUserSecrets(typeof(Program).Assembly);
        return builder.Build();
    }

    private static void ConfigureService(
        IConfigurationRoot configuration,
        IServiceCollection services) {
        services.AddOptions();
        var server = configuration.GetValue<string>("Server");
        System.Console.Out.WriteLine($"server: {server}");

#if false
    services.AddHttpClient<TypedClient>()
        .ConfigureHttpClient((sp, httpClient) =>
        {
            var options = sp.GetRequiredService<IOptions<SomeOptions>>().Value;
            httpClient.BaseAddress = options.Url;
            httpClient.Timeout = options.RequestTimeout;
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler() 
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        })
        .AddHttpMessageHandler(sp => sp.GetService<SomeCustomHandler>().CreateAuthHandler())
        .AddPolicyHandlerFromRegistry(PollyPolicyName.HttpRetry)
        .AddPolicyHandlerFromRegistry(PollyPolicyName.HttpCircuitBreaker);
#endif
        //services.AddHttpClient<Replacement.Client.IReplacementClient, Replacement.Client.ReplacementClient>();

        services.AddHttpClient<Replacement.Client.IReplacementClient, Replacement.Client.ReplacementClient>(
            (HttpClient httpClient, IServiceProvider sp) => {
                //httpClient.BaseAddress = new Uri(server);
                return new Replacement.Client.ReplacementClient(server, httpClient);
            })
            .ConfigureHttpClient((sp, httpClient) => {
                httpClient.BaseAddress = new Uri(server);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .ConfigurePrimaryHttpMessageHandler((sp) => {
                var result = new HttpClientHandler() {
                    UseDefaultCredentials = true
                };
                return result;
            })
            ;
    }

    private static async Task<int> Run(ServiceProvider appServiceProvider) {
        var dtStart = System.DateTime.UtcNow;
        try {
            IReplacementClient client = appServiceProvider.GetRequiredService<IReplacementClient>();
            List<Guid> lstProjectId = new List<Guid>();
            var dctProject = new Dictionary<ProjectPK, Project>();

            for (int idxOuter = 0; idxOuter < cntOuter; idxOuter++) {
                System.Console.Out.WriteLine($"idxOuter : {idxOuter} / {cntOuter} : {lstProjectId.Count}");

                for (int idxWrite = 0; idxWrite < cntInnerWrite; idxWrite++) {
                    var projectId = Guid.NewGuid();
                    lstProjectId.Add(projectId);
                    var projectA = new Project(
                            ProjectId: projectId,
                            Title: projectId.ToString(),
                            OperationId: Guid.Empty,
                            CreatedAt: new System.DateTimeOffset(),
                            CreatedBy: null,
                            ModifiedAt: new System.DateTimeOffset(),
                            ModifiedBy: null,
                            SerialVersion: 0
                            );
                    var projectB = await client.ProjectPostAsync(projectA);
                    System.Threading.Interlocked.Increment(ref cntProjectWrite);
                    dctProject[projectB.GetPrimaryKey()] = projectB;
                }
                //
                for (int idxRead = 0; idxRead < cntInnerRead; idxRead++) {
                    int idxInner = Random.Shared.Next(lstProjectId.Count);
                    var projectId = lstProjectId[idxInner];
                    var projectA = dctProject[new ProjectPK(projectId)];
                    var projectB = await client.ProjectGetOneAsync(projectId);
                    System.Threading.Interlocked.Increment(ref cntProjectRead);
                    if (projectA.ProjectId == projectB.ProjectId) {
                        // OK
                    } else {
                        System.Console.Error.WriteLine($"idxInner:{idxInner} {projectA.ProjectId}--{projectB.ProjectId}");
                        return 1;
                    }
                }
                //
                for (int idxUpdate = 0; idxUpdate < cntInnerUpdate; idxUpdate++) {
                    int idxInner = Random.Shared.Next(lstProjectId.Count);
                    var projectId = lstProjectId[idxInner];
                    var projectA = dctProject[new ProjectPK(projectId)];
                    projectA = projectA with { Title = System.DateTime.Now.ToString() };
                    var projectB = await client.ProjectPostAsync(projectA);
                    dctProject[projectB.GetPrimaryKey()] = projectB;
                    System.Threading.Interlocked.Increment(ref cntProjectUpdate);
                    if (projectA.ProjectId == projectB.ProjectId) {
                        // OK
                    } else {
                        System.Console.Error.WriteLine($"idxInner:{idxInner} {projectA.ProjectId}--{projectB.ProjectId}");
                        return 1;
                    }
                }
                //
                for (int idxRead = 0; idxRead < cntInnerRead; idxRead++) {
                    int idxInner = Random.Shared.Next(lstProjectId.Count);
                    var projectId = lstProjectId[idxInner];
                    var projectA = dctProject[new ProjectPK(projectId)];
                    var projectB = await client.ProjectGetOneAsync(projectId);
                    System.Threading.Interlocked.Increment(ref cntProjectRead);
                    if (projectA.ProjectId == projectB.ProjectId) {
                        // OK
                    } else {
                        System.Console.Error.WriteLine($"idxInner:{idxInner} {projectA.ProjectId}--{projectB.ProjectId}");
                        return 1;
                    }
                }
                //
            }
            
            return 0;
        } catch (System.Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            return 1;
        } finally {
            var dtEnd = System.DateTime.UtcNow;
            var tsDuration = dtEnd.Subtract(dtStart);
            lock (typeof(Program)) {
                tsTotal += tsDuration;
            }
        }
    }

}
