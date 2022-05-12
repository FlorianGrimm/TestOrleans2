namespace TestOrleans2.DatabaseDevTool;

public class GenerateConverter {
    public GenerateConverter() {
    }

    public bool GenerateConverterToAPI(DefineMapping defineMapping, string outputPathConverterToAPI) {
        var output = new StringBuilder();

        output.AppendLine("namespace TestOrleans2.Contracts.Entity;");
        output.AppendLine("[ExcludeFromCodeCoverage]");
        output.AppendLine("public static partial class ConverterToAPI {");
        foreach (var typeMapping in defineMapping.TypeMappings) {
            var nameAPI = typeMapping.TypeAPI.Name;
            var nameEntity = typeMapping.TypeEntity.Name;
            output.AppendLine("    [return: NotNullIfNotNull(\"that\")]");
            output.AppendLine($"    public static {nameAPI}? To{nameAPI}(this {nameEntity}? that) {{");
            output.AppendLine("        if (that is null) {");
            output.AppendLine("            return default;");
            output.AppendLine("        } else {");
            output.AppendLine($"            return new {nameAPI}(");

            var idxPropertyLast = typeMapping.PropertyMappings.Length - 1;
            for (int idxProperty = 0; idxProperty < typeMapping.PropertyMappings.Length; idxProperty++) {
                var propertyMapping = typeMapping.PropertyMappings[idxProperty];
                var suffix = (idxProperty < idxPropertyLast) ? "," : String.Empty;
                if (!string.IsNullOrEmpty(propertyMapping.ConvertToAPI)) {
                    output.AppendLine($"                {propertyMapping.ConvertToAPI}{suffix}");
                } else {
                    output.AppendLine($"                {propertyMapping.PropertyAPI.Name}: that.{propertyMapping.PropertyEntity.Name}{suffix}");
                }
            }
            output.AppendLine("                );");
            output.AppendLine("        }");
            output.AppendLine("    }");
            output.AppendLine("");
            output.AppendLine($"    public static List<{nameAPI}> ToList{nameAPI}(this IEnumerable<{nameEntity}> that) {{");
            output.AppendLine($"        var result = new List<{nameAPI}>();");
            output.AppendLine("        foreach (var item in that) { ");
            output.AppendLine($"            result.Add(item.To{nameAPI}());");
            output.AppendLine("        }");
            output.AppendLine("        return result;");
            output.AppendLine("    }");
            output.AppendLine("");
        }
        output.AppendLine("}");

        return this.WriteFile(outputPathConverterToAPI, output.ToString());
    }

    public bool GenerateConverterToEntity(DefineMapping defineMapping, string outputPathConverterToEntity) {
        var output = new StringBuilder();

        output.AppendLine("namespace TestOrleans2.Contracts.API;");
        output.AppendLine("[ExcludeFromCodeCoverage]");
        output.AppendLine("public static partial class ConverterToEntity {");
        foreach (var typeMapping in defineMapping.TypeMappings) {
            var nameAPI = typeMapping.TypeAPI.Name;
            var nameEntity = typeMapping.TypeEntity.Name;
            output.AppendLine("    [return: NotNullIfNotNull(\"that\")]");
            output.AppendLine($"    public static {nameEntity}? To{nameEntity}(this {nameAPI}? that) {{");
            output.AppendLine("        if (that is null) {");
            output.AppendLine("            return default;");
            output.AppendLine("        } else {");
            output.AppendLine($"            return new {nameEntity}(");

            var idxPropertyLast = typeMapping.PropertyMappings.Length - 1;
            for (int idxProperty = 0; idxProperty < typeMapping.PropertyMappings.Length; idxProperty++) {
                var propertyMapping = typeMapping.PropertyMappings[idxProperty];
                var suffix = (idxProperty < idxPropertyLast) ? "," : String.Empty;
                if (!string.IsNullOrEmpty(propertyMapping.ConvertToEntity)) {
                    output.AppendLine($"                {propertyMapping.ConvertToEntity}{suffix}");
                } else {
                    output.AppendLine($"                {propertyMapping.PropertyEntity.Name}: that.{propertyMapping.PropertyAPI.Name}{suffix}");
                }
            }
            output.AppendLine("                );");
            output.AppendLine("        }");
            output.AppendLine("    }");
            output.AppendLine("");
            output.AppendLine($"    public static List<{nameEntity}> ToList{nameEntity}(this IEnumerable<{nameAPI}> that) {{");
            output.AppendLine($"        var result = new List<{nameEntity}>();");
            output.AppendLine("        foreach (var item in that) { ");
            output.AppendLine($"            result.Add(item.To{nameEntity}());");
            output.AppendLine("        }");
            output.AppendLine("        return result;");
            output.AppendLine("    }");
            output.AppendLine("");
        }
        output.AppendLine("}");

        return this.WriteFile(outputPathConverterToEntity, output.ToString());
    }

    private bool WriteFile(string outputPath, string output) {
        string current = string.Empty;
        try {
            current = System.IO.File.ReadAllText(outputPath);
        } catch {
            current = string.Empty;
        }
        if (string.Equals(current, output, StringComparison.Ordinal)) {
            System.Console.WriteLine($"OK     : {outputPath}");
            return false;
        } else {
            System.IO.File.WriteAllText(outputPath, output);
            System.Console.WriteLine($"changed: {outputPath}");
            return true;
        }
    }
}