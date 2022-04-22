using Microsoft.OpenApi.Models;

namespace Replacement.WebApp.Swagger {
    public class SwaggerOptions
    {
        public SwaggerOptions()
        {
            this.DocumentName = string.Empty;
            this.OutputPath = string.Empty;
            this.Host = string.Empty;
            this.Basepath = string.Empty;
            this.OpenApiInfo = new OpenApiInfo();
        }

        public bool Generate { get; set; }
        public string DocumentName { get; set; }
        public string OutputPath { get; set; }
        public bool Yaml { get; set; }
        public bool SerializeasV2 { get; set; }
        public string Host { get; set; }
        public string Basepath { get; set; }
        public OpenApiInfo OpenApiInfo { get; set; }
    }
}
