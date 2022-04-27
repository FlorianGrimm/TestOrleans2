using Brimborium.TypedStoredProcedure;

using Microsoft.Extensions.Configuration;

namespace Replacement.DatabaseDevTool;

public static partial class Program {
    public static int Main(string[] args) {
        bool isForce = args.Contains("--force");
        if (isForce) {
            args = args.Where(a => (a != "--force")).ToArray();
            System.Console.Out.WriteLine("update with --force");
        }
        try {
            var configuration = GetConfiguration(args);

            var connectionString = configuration.GetValue<string>("ConnectionString");
            var outputFolder = configuration.GetValue<string>("OutputFolder")
                ?? configuration.GetValue<string>("App:OutputFolder");
            var sqlProjectName = configuration.GetValue<string>("SqlProject");
            var sqlProjectTablesName = configuration.GetValue<string>("SqlProjectTables");

            if (string.IsNullOrEmpty(connectionString)) {
                System.Console.Error.WriteLine("ConnectionString is empty");
                return 1;
            }
            // System.Console.Out.WriteLine(connectionString);

            var upperDirectoryPath = GetUpperDirectoryPath();

            if (string.IsNullOrEmpty(outputFolder)) {
                outputFolder = "Replacement.Database"; // change this
            }
            if (string.IsNullOrEmpty(sqlProjectName)) {
                sqlProjectName = "Replacement.DatabaseDeploy"; // change this
            }
            if (string.IsNullOrEmpty(sqlProjectTablesName)) {
                sqlProjectTablesName = "Replacement.DatabaseTablesDeploy"; // change this
            }

            if (string.IsNullOrEmpty(outputFolder)) {
                System.Console.Error.WriteLine("outputFolder is empty");
                return 1;
            }

            // MainGenerateSql
            bool changes = false;

            var dotnetPath = Brimborium.GenerateStoredProcedure.Utility.TryGetDotNetPath();
            if (string.IsNullOrEmpty(dotnetPath)) {
                System.Console.Error.WriteLine("dotnet not found");
                return 1;
            }
            {
                var sqlProjectTables_csproj = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectTablesName,
                    $"{sqlProjectTablesName}.csproj"
                    );
                var sqlProjectTablesDirectory = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectTablesName
                    );

                System.Console.Out.WriteLine($"Update tables in database.");
                var subresult = UpdateDatabase(connectionString, dotnetPath, sqlProjectTables_csproj, sqlProjectTablesDirectory, isForce);
                if (subresult != 0) { return subresult; }
            }
#if true
            {
                if (!System.IO.Path.IsPathFullyQualified(outputFolder)) {
                    outputFolder = System.IO.Path.Combine(upperDirectoryPath, outputFolder);
                }

                changes = MainGenerateSql(connectionString, outputFolder, isForce);

                //changes = true;
            }
#else
            changes = true;
#endif

#if true
            {
                var sqlProjectComplete_csproj = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectName,
                    $"{sqlProjectName}.csproj"
                    );
                var sqlProjectCompleteDirectory = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectName
                    );
                if (changes) {
                    System.Console.Out.WriteLine($"changes try to update.");
                }
                if (changes || isForce) {
                    var subresult = UpdateDatabase(connectionString, dotnetPath, sqlProjectComplete_csproj, sqlProjectCompleteDirectory, isForce);
                    if (subresult != 0) { return subresult; }
                } else {
                    System.Console.Out.WriteLine($"no changes in sql.");
                }
            }

#endif
            // GenerateSqlAccess
#if true
            {
                AddNativeTypeConverter();

                {
                    var (outputPath, outputNamespace) = Replacement.Contracts.API.PrimaryKeyLocation.GetPrimaryKeyOutputInfo();
                    var subResult = MainGeneratePrimaryKey(connectionString, outputPath, outputNamespace);
                    if (subResult) {
                        System.Console.Out.WriteLine("PrimaryKey modified.");
                        System.Console.Out.WriteLine("Please rerun.");
                        System.Console.Out.WriteLine("terminate");
                        return 0;
                    }
                }
                {
                    var defintions = GetDefintion();

                    var (outputPath, outputNamespace, outputClassName) = Replacement.Repository.Service.SqlAccessLocation.GetPrimaryKeyOutputInfo();

                    MainGenerateSqlAccess(connectionString, defintions, outputPath, outputNamespace, outputClassName, isForce);
                }
            }
#endif

            System.Console.Out.WriteLine($"done.");

            return 0;
        } catch (System.Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            return 1;
        }
    }

    private static int UpdateDatabase(
        string connectionString,
        string dotnetPath,
        string sqlProject_csproj,
        string sqlProjectDirectory,
        bool isForce) {
        var diBin = new System.IO.DirectoryInfo(
            System.IO.Path.Combine(
                sqlProjectDirectory,
                "bin"
                )
            );

        var dacpacFilesBefore = diBin.EnumerateFiles(
            System.IO.Path.GetFileNameWithoutExtension(sqlProject_csproj) + ".dacpac",
            System.IO.SearchOption.AllDirectories)
            .Select(fi => new SavedFileInfo(fi.FullName, fi.LastWriteTimeUtc))
            .ToList();

        // dotnet build
        {
            var psi = new System.Diagnostics.ProcessStartInfo(
                dotnetPath, $"build \"{sqlProject_csproj}\"");
            psi.WorkingDirectory = sqlProjectDirectory;
            var process = System.Diagnostics.Process.Start(psi);
            if (process is not null) {
                process.WaitForExit(30_000);
                if (process.ExitCode == 0) {
                    System.Console.Out.WriteLine($"dotnet build {sqlProject_csproj} OK");
                } else {
                    System.Console.Error.WriteLine($"dotnet build {sqlProject_csproj} Failed");
                    return 1;
                }
            }
        }
        var dacpacFilesAfter = diBin.EnumerateFiles(
            System.IO.Path.GetFileNameWithoutExtension(sqlProject_csproj) + ".dacpac",
            System.IO.SearchOption.AllDirectories)
            .Select(fi => new SavedFileInfo(fi.FullName, fi.LastWriteTimeUtc))
            .ToList();
        var dacpacChanged =
            (dacpacFilesBefore.Count != dacpacFilesAfter.Count)
            && (dacpacFilesBefore.Join(
                    dacpacFilesAfter,
                    o => o.FullName,
                    i => i.FullName,
                    (o, i) => o.LastWriteTimeUtc != o.LastWriteTimeUtc)
                    .Any(c => c)
            );

        // dotnet publish
        if (dacpacChanged || isForce) {
            System.Console.Out.WriteLine($"sqlProject: {sqlProject_csproj}");

            var csb = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var p1 = string.IsNullOrEmpty(csb.DataSource) || string.IsNullOrEmpty(csb.InitialCatalog) ? string.Empty :
                $"/p:TargetServerName=\"{csb.DataSource}\" /p:TargetDatabaseName=\"{csb.InitialCatalog}\"";
            var p2 = string.IsNullOrEmpty(csb.UserID) || string.IsNullOrEmpty(csb.Password) ? string.Empty :
                $"/p:TargetUser=\"{csb.UserID} /p:TargetPassword=\"{csb.Password} ";
            var psi = new System.Diagnostics.ProcessStartInfo(
                dotnetPath,
                $"publish \"{sqlProject_csproj}\" {p1} {p2}");
            psi.WorkingDirectory = sqlProjectDirectory;
            var process = System.Diagnostics.Process.Start(psi);
            if (process is not null) {
                process.WaitForExit(30_000);
                if (process.ExitCode == 0) {
                    System.Console.Out.WriteLine($"dotnet publish {sqlProject_csproj} OK");
                } else {
                    System.Console.Out.WriteLine($"dotnet publish {sqlProject_csproj} Failed");
                    return 1;
                }
            }
            return 0;
        } else {
            return 0;
        }
    }

    public static IConfigurationRoot GetConfiguration(string[] args) {
        var configurationBuilder = new ConfigurationBuilder();
        // configurationBuilder.AddJsonFile(@"............json", true);
        configurationBuilder.AddCommandLine(args).AddUserSecrets(assembly: typeof(Program).Assembly, optional: true);
        var configuration = configurationBuilder.Build();
        return configuration;
    }

    public static bool MainGenerateSql(string connectionString, string outputFolder, bool isForce) {
        var templateVariables = new Dictionary<string, string>();
        var cfg = new GenerateConfiguration();
        return Brimborium.GenerateStoredProcedure.Generator.GenerateSql(
            connectionString,
            outputFolder,
            cfg,
            templateVariables, 
            isForce);
    }

    private static bool MainGeneratePrimaryKey(
        string connectionString,
        string outputFilePrimaryKey,
        string outputNamespacePrimaryKey
        ) {
        var printClass = new PrintClass(outputNamespacePrimaryKey, string.Empty);
        var result = Generator.GenerateModel(connectionString, outputFilePrimaryKey, printClass);
        return result;
    }

    private static void MainGenerateSqlAccess(
        string connectionString,
        DatabaseDefintion dbDefs,
        string outputPath,
        string outputNamespace,
        string outputClassName, 
        bool isForce
        ) {
        var refType = typeof(Replacement.Contracts.API.PrimaryKeyLocation);
        var refTypeNamespace = refType.Namespace;
        var types = refType.Assembly.GetTypes()
            .Where(t => t.Namespace == refTypeNamespace)
            .ToArray()
            ;
        var printClass = new PrintClass(
            outputNamespace,
            outputClassName
            );
        Generator.GenerateSqlAccessWrapper(types, connectionString, outputPath, dbDefs, printClass, isForce);
    }

    public static string GetUpperDirectoryPath() {
        return System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(getFilePathGenerated() ?? string.Empty)!)!;
        static string? getFilePathGenerated([System.Runtime.CompilerServices.CallerFilePath] string? fp = default) => fp;
    }
}
record SavedFileInfo(string FullName, DateTime LastWriteTimeUtc);
