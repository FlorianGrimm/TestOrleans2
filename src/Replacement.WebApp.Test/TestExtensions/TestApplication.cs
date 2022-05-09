using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Replacement.TestExtensions;

public class TestApplication : IHttpApplication<HttpContext> {
    private readonly RequestDelegate _requestDelegate;

    //public TestApplication() : this(context => Task.CompletedTask) { }

    public TestApplication(RequestDelegate requestDelegate) {
        _requestDelegate = requestDelegate;
    }

    public HttpContext CreateContext(IFeatureCollection contextFeatures) {
        return new DefaultHttpContext(contextFeatures);
    }

    public void DisposeContext(HttpContext httpContext, Exception? exception) {
        if (httpContext is IDisposable d) {
            d.Dispose();
        }
    }

    public async Task ProcessRequestAsync(HttpContext httpContext) {
        await _requestDelegate(httpContext);
    }


    public static TestApplication Create(IServer Server, IServiceProvider applicationServices, Action<Microsoft.AspNetCore.Builder.IApplicationBuilder> configure) {
        try {
            var builderFactory = applicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory>();
            var builder = builderFactory.CreateBuilder(Server.Features);
            builder.ApplicationServices = applicationServices;
            
            //var startupFilters = applicationServices.GetService<IEnumerable<IStartupFilter>>();
            //Action<IApplicationBuilder> configure = _startup!.Configure;
            //if (startupFilters != null) {
            //    foreach (var filter in startupFilters.Reverse()) {
            //        configure = filter.Configure(configure);
            //    }
            //}

            configure(builder);

            RequestDelegate requestDelegate = builder.Build();
            return new TestApplication(requestDelegate);
        } catch (Exception ex) {
            // Write errors to standard out so they can be retrieved when not in development mode.
            System.Console.WriteLine("Application startup exception: " + ex.ToString());

            throw;
        }
    }
}
