using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

using System.Diagnostics;

namespace Replacement.TestExtensions;

[CollectionDefinition("DefaultClusterFixture")]
public class DefaultClusterFixture : BaseClusterFixture {
    public DefaultClusterFixture() {
    }

    public override void ConfigureTestClusterBuilder(TestClusterBuilder builder) {
        TestDefaultConfiguration.ConfigureTestCluster(builder);
        builder.AddSiloBuilderConfigurator<SiloHostConfigurator>();
        /*
            builder.AddSiloBuilderConfigurator<CoreHostConfigurator>();
            builder.AddSiloBuilderConfigurator<TestHostConfigurator>();
            builder.AddClientBuilderConfigurator<CoreClientBuilderConfigurator>();
            builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();
        */        
    }
#if false
    public class CoreHostConfigurator : IHostConfigurator {
        public void Configure(Microsoft.Extensions.Hosting.IHostBuilder hostBuilder) {
            hostBuilder.ConfigureServices((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
                var x = hostBuilderContext.HostingEnvironment;
                var xx = x is Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

                services.TryAddSingleton(sp => new DiagnosticListener("Microsoft.AspNetCore"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("Microsoft.AspNetCore"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                services.AddTransient<IApplicationBuilderFactory, ApplicationBuilderFactory>();
                services.AddTransient<IHttpContextFactory, DefaultHttpContextFactory>();
                services.AddScoped<IMiddlewareFactory, MiddlewareFactory>();
                services.AddOptions();
                services.AddLogging();

                services.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();

            });
        }
        //class TestWebHostEnvironment : Microsoft.AspNetCore.Hosting.IWebHostEnvironment {
        //    private readonly IHostEnvironment _HostEnvironment;

        //    public TestWebHostEnvironment(
        //        IHostEnvironment hostEnvironment
        //        ) {
        //        this._HostEnvironment = hostEnvironment;
        //    }

        //    public string ApplicationName { get => this._HostEnvironment.ApplicationName; set => this._HostEnvironment.ApplicationName = value; }
        //    public IFileProvider ContentRootFileProvider { get => this._HostEnvironment.ContentRootFileProvider; set => this._HostEnvironment.ContentRootFileProvider = value; }
        //    public string ContentRootPath { get => this._HostEnvironment.ContentRootPath; set => this._HostEnvironment.ContentRootPath = value; }
        //    public string EnvironmentName { get => this._HostEnvironment.EnvironmentName; set => this._HostEnvironment.EnvironmentName = value; }
        //    public string WebRootPath { get => this._HostEnvironment.W; set => throw new NotImplementedException(); }
        //    public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //}
    }

    public class TestHostConfigurator : IHostConfigurator {
        public void Configure(Microsoft.Extensions.Hosting.IHostBuilder hostBuilder) {
            hostBuilder.ConfigureServices((Microsoft.Extensions.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
                var startup = new Replacement.WebApp.Startup(hostBuilderContext.Configuration);
                services.TryAddSingleton(startup);
                //startup.ConfigureServices(services);

                startup.AddAppServicesWithRegistrator(services);
                startup.AddAppOptions(services);
            });
        }
    }
#endif

    public class SiloHostConfigurator : ISiloConfigurator {
        public void Configure(ISiloBuilder hostBuilder) {
            hostBuilder
                .UseInMemoryReminderService()
                .AddMemoryGrainStorageAsDefault()
                .AddMemoryGrainStorage("MemoryStore");
        }
    }

#if false
    public class CoreClientBuilderConfigurator : IClientBuilderConfigurator {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder) {
            clientBuilder.ConfigureServices((Orleans.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
                var x = hostBuilderContext.HostingEnvironment;
                var xx = x is Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

                services.TryAddSingleton(sp => new DiagnosticListener("Microsoft.AspNetCore"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("Microsoft.AspNetCore"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                services.AddTransient<IApplicationBuilderFactory, ApplicationBuilderFactory>();
                services.AddTransient<IHttpContextFactory, DefaultHttpContextFactory>();
                services.AddScoped<IMiddlewareFactory, MiddlewareFactory>();
                services.AddOptions();
                services.AddLogging();

                services.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();
            });
        }
    }

    public class TestClientBuilderConfigurator : IClientBuilderConfigurator {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder) {
            clientBuilder.ConfigureServices((Orleans.Hosting.HostBuilderContext hostBuilderContext, IServiceCollection services) => {
                var startup = new Replacement.WebApp.Startup(hostBuilderContext.Configuration);
                services.TryAddSingleton(startup);
                //startup.ConfigureServices(services);

                startup.AddAppServicesWithRegistrator(services);
                startup.AddAppOptions(services);
            });
        }
    }
#endif
}

#if false


#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
internal class HostingEnvironment : Microsoft.AspNetCore.Hosting.IHostingEnvironment,  Microsoft.Extensions.Hosting.IHostingEnvironment, Microsoft.AspNetCore.Hosting.IWebHostEnvironment
{
    private readonly Orleans.Hosting.IHostingEnvironment _HostingEnvironment;

    public HostingEnvironment(Orleans.Hosting.IHostingEnvironment hostingEnvironment) {
        this._HostingEnvironment = hostingEnvironment;
        this.EnvironmentName = hostingEnvironment.EnvironmentName;
        this.ApplicationName = hostingEnvironment.ApplicationName;
    }

    public HostingEnvironment() {
        this.EnvironmentName = Microsoft.Extensions.Hosting.Environments.Production;
    }

    public string EnvironmentName { get; set; } ;

    public string? ApplicationName { get; set; }

    public string WebRootPath { get; set; } = default!;

    public IFileProvider WebRootFileProvider { get; set; } = default!;

    public string ContentRootPath { get; set; } = default!;

    public IFileProvider ContentRootFileProvider { get; set; } = default!;
}

#endif