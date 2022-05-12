namespace TestOrleans2.WebApp.Swagger {
    using Microsoft.OpenApi.Writers;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// swagger:DocumentName - as defined in code otherwise run webapp.
    /// swagger:OutputPath - output filepath otherwise Console.Out.
    /// swagger:yaml true:yaml otherwise json.
    /// swagger:serializeasv2 - true:v2 otherwise v3.
    /// swagger:host - host in swagger.
    /// swagger:basepath - basepath in swagger.
    /// </summary>
    public static class SwaggerGenerator {
        public static bool Generate(
            string[] args,
            IHostBuilder hostBuilder,
            SwaggerOptions? swaggerOptions = default,
            Action<IHostBuilder>? configureForSwaggerGeneration = default
            ) {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationSource() { Args = args });
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource() { Prefix = "swagger:" });
            var configuration = configurationBuilder.Build();

            if (swaggerOptions is null) {
                swaggerOptions = new SwaggerOptions();
            }
            configuration.GetSection("Swagger").Bind(swaggerOptions);

            if (swaggerOptions.Generate) {
                if (configureForSwaggerGeneration is not null) {
                    configureForSwaggerGeneration(hostBuilder);
                }
                var serviceProvider = hostBuilder.Build().Services;
                var swaggerProvider = serviceProvider.GetRequiredService<ISwaggerProvider>();
                var swagger = swaggerProvider.GetSwagger(
                    documentName: swaggerOptions.DocumentName,
                    host: string.IsNullOrEmpty(swaggerOptions.Host) ? null : swaggerOptions.Host,
                    basePath: string.IsNullOrEmpty(swaggerOptions.Basepath) ? null : swaggerOptions.Basepath
                    );
                var outputPath = string.IsNullOrEmpty(swaggerOptions.OutputPath)
                        ? null
                        : System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), swaggerOptions.OutputPath)
                        ;
                if (string.IsNullOrEmpty(outputPath)) {
                    var swSwagger = System.Console.Out;
                    IOpenApiWriter writer = (swaggerOptions.Yaml)
                        ? new OpenApiYamlWriter(swSwagger)
                        : writer = new OpenApiJsonWriter(swSwagger);
                    if (swaggerOptions.SerializeasV2) {
                        swagger.SerializeAsV2(writer);
                    } else {
                        swagger.SerializeAsV3(writer);
                    }
                } else {
                    var sbSwagger = new StringBuilder();
                    using (var swSwagger = new System.IO.StringWriter(sbSwagger)) {
                        IOpenApiWriter writer = (swaggerOptions.Yaml)
                            ? new OpenApiYamlWriter(swSwagger)
                            : writer = new OpenApiJsonWriter(swSwagger);
                        if (swaggerOptions.SerializeasV2) {
                            swagger.SerializeAsV2(writer);
                        } else {
                            swagger.SerializeAsV3(writer);
                        }
                    }
                    string contentNew = sbSwagger.ToString();
                    string contentOld;
                    {
                        try {
                            contentOld = System.IO.File.ReadAllText(outputPath);
                        } catch (System.IO.FileNotFoundException) {
                            contentOld = string.Empty;
                        }
                    }
                    if (string.Equals(contentOld, contentNew, StringComparison.Ordinal)) {
                        System.Console.Out.WriteLine($"Swagger {(swaggerOptions.SerializeasV2 ? "V2" : "V3")} {(swaggerOptions.Yaml ? "YAML" : "JSON")} is uptodate to {outputPath}.");
                    } else {
                        System.IO.File.WriteAllText(outputPath, contentNew);
                        System.Console.Out.WriteLine($"Swagger {(swaggerOptions.SerializeasV2 ? "V2" : "V3")} {(swaggerOptions.Yaml ? "YAML" : "JSON")} successfully written to {outputPath}.");
                    }
                }
                return true;
            } else {
                return false;
            }
        }
    }
}
