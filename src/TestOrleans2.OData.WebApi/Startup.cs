namespace TestOrleans2.OData;
public class Startup {
    public Startup(IConfiguration configuration) {
        this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

        services
            .AddControllers()
            .AddOData((oDataOptions, serviceProvider) => {
                oDataOptions.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000);
                oDataOptions.AddRouteComponents("odata", GetEdmModel(serviceProvider));
            });
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestOrleans2.OData", Version = "v1" });
        });

        services.AddDbContext<TodoDBContext>(options => {
            var connectionString = this.Configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(connectionString)) {
                connectionString = this.Configuration.GetValue<string>("Database");
            }
            if (string.IsNullOrEmpty(connectionString)) {
                //connectionString = "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;";

                connectionString = "Data Source=parado.dev.solvin.local;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;";
            }
            options.UseSqlServer(connectionString);
            /*
             "Database": "Data Source=.;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;"
             "Database": "Data Source=parado.dev.solvin.local;Initial Catalog=TodoDB;Integrated Security=true;TrustServerCertificate=True;"
             */
        });

        services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestOrleans2.OData v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });
    }
    
    private static IEdmModel GetEdmModel(IServiceProvider serviceProvider) {
        /*
        var todoDBContext = serviceProvider.GetRequiredService<TodoDBContext>();
        var model = todoDBContext.Model;
        foreach(var entityType in model.GetEntityTypes()) {
            entityType.ClrType
        }
        */
        var assemblyResolver = new Microsoft.OData.ModelBuilder.DefaultAssemblyResolver();
        var builder = new ODataConventionModelBuilder(assemblyResolver);
        
        builder.EntitySet<User>("User");
        builder.EntitySet<Project>("Project");
        builder.EntitySet<ToDo>("ToDo");
        
        return builder.GetEdmModel();
    }
}
