using System.Net.Http;

namespace Replacement.Console;

public static class Program {
    public static async Task<int> Main(string[] args) {
        System.Console.Out.WriteLine("Replacement.Console!");
        var configuration = ConfigureApp(args);
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        ConfigureService(configuration, services);
        var appServiceProvider = services.BuildServiceProvider();
        return await Run(appServiceProvider);
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
        try {
            var dtStart = System.DateTime.UtcNow;
            System.Console.Out.WriteLine($"dtStart : {dtStart}");

            var cntWrite = 0;
            var cntRead = 0;
            IReplacementClient client = appServiceProvider.GetRequiredService<IReplacementClient>();
            List<Guid> lstProjectId = new List<Guid>();
            var dctProject = new Dictionary<ProjectPK, Project>();
            int cntOuter = 100;
            for (int idxOuter = 0; idxOuter < cntOuter; idxOuter++) {
                System.Console.Out.WriteLine($"idxOuter : {idxOuter} / {cntOuter} : {lstProjectId.Count}");

                for (int idxWrite = 0; idxWrite < 10; idxWrite++) {
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
                    cntWrite++;
                    dctProject[projectB.GetPrimaryKey()] = projectB;
                }
                for (int idxRead = 0; idxRead < 40; idxRead++) {
                    int idxInner = Random.Shared.Next(lstProjectId.Count);
                    var projectId = lstProjectId[idxInner];
                    var projectA = dctProject[new ProjectPK(projectId)];
                    var projectB = await client.ProjectGetOneAsync(projectId);
                    cntRead++;
                    if (projectA.ProjectId == projectB.ProjectId) {
                        // OK
                    } else {
                        System.Console.Error.WriteLine($"idxInner:{idxInner} {projectA.ProjectId}--{projectB.ProjectId}");
                        return 1;
                    }
                }
                for (int idxRead = 0; idxRead < 10; idxRead++) {
                    int idxInner = Random.Shared.Next(lstProjectId.Count);
                    var projectId = lstProjectId[idxInner];
                    var projectA = dctProject[new ProjectPK(projectId)];
                    projectA = projectA with { Title = System.DateTime.Now.ToString() };
                    var projectB = await client.ProjectPostAsync(projectA);
                    dctProject[projectB.GetPrimaryKey()] = projectB;
                    cntRead++;
                    if (projectA.ProjectId == projectB.ProjectId) {
                        // OK
                    } else {
                        System.Console.Error.WriteLine($"idxInner:{idxInner} {projectA.ProjectId}--{projectB.ProjectId}");
                        return 1;
                    }
                }
            }
            var dtEnd = System.DateTime.UtcNow;
            System.Console.Out.WriteLine($"duration: {dtEnd.Subtract(dtStart).TotalSeconds} sec");
            System.Console.Out.WriteLine($"cntWrite: {cntWrite}");
            System.Console.Out.WriteLine($"cntRead : {cntRead}");
            return 0;
        } catch (System.Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            return 1;
        }

    }

}
