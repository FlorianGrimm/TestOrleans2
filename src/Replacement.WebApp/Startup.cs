using Replacement.Contracts.API;
using Replacement.Repository.Service;

namespace Replacement.WebApp;

public class Startup {
    private static SwaggerOptions? _SwaggerOptions;
    public static SwaggerOptions GetSwaggerOptions()
        => _SwaggerOptions ??= new SwaggerOptions() {
            DocumentName = "v1",
            OpenApiInfo = new OpenApiInfo {
                Title = "Replacement.WebApp",
                Version = "v1"
            }
        };
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
        var mvcBuilderControllers = this.AddControllers(services);
        // AddAppOData(mvcBuilderControllers);
        services.AddRazorPages();
        AddAppSwaggerGen(services);
        AddAppServicesWithRegistrator(services);
        AddAppAuthentication(services);
        services.AddOptions<DBContextOption>().Configure(options => {
            options.ConnectionString = this.Configuration.GetValue<string>("Database");
        });
    }

   

    public void ConfigureSwaggerGeneratorServices(IServiceCollection services) {
        this.AddControllers(services);
        AddAppSwaggerGen(services);
        AddAppServicesWithRegistrator(services);
    }

    private IMvcBuilder AddControllers(IServiceCollection services) {
        return services.AddControllers((Microsoft.AspNetCore.Mvc.MvcOptions options) => {
            options.RespectBrowserAcceptHeader = true;
        });
    }

    private static void AddAppSwaggerGen(IServiceCollection services) {
        var swaggerOptions = GetSwaggerOptions();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc(swaggerOptions.DocumentName, swaggerOptions.OpenApiInfo);
        });
    }

    private static void AddAppServicesWithRegistrator(IServiceCollection services) {
        services.AddServicesWithRegistrator(
            actionAdd: (typeSourceSelector) => {
                services.AddAttributtedServices(
                    implementationTypeSelector: typeSourceSelector.FromApplicationDependencies(
                        Microsoft.Extensions.DependencyModel.DependencyContext.Default));
            },
            actionRevisit: (selectorTarget) => {
            });
    }

    private void AddAppAuthentication(IServiceCollection services) {
        //services.AddAuthorization()
        services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate((NegotiateOptions negotiateOptions) => {
            });

        services.AddAuthorization(options => {
            options.FallbackPolicy = options.DefaultPolicy;
        });
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var swaggerOptions = GetSwaggerOptions();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{swaggerOptions.DocumentName}/swagger.json", swaggerOptions.OpenApiInfo.Title));

        app.UseRouting();

        app.UseAuthentication();
        
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });
    }
}