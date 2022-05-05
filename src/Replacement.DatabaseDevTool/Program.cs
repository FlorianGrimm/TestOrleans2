using Brimborium.TypedStoredProcedure;

namespace Replacement.DatabaseDevTool;

public static partial class Program {

    public static int Main(string[] args) {
        if (args.Contains("--help")) {
            System.Console.Out.WriteLine("dotnet run -- --force --verbose --steps 1,2,3,4,5");
            System.Console.Out.WriteLine("");
            System.Console.Out.WriteLine("--ConnectionString - required.");
            System.Console.Out.WriteLine("--OutputFolder     - for the sql files.");
            System.Console.Out.WriteLine("--force true       - force update database.");
            System.Console.Out.WriteLine("--verbose true");
            System.Console.Out.WriteLine("--steps 1,2,3,4,5,6");
            printSteps(0, new HashSet<int>());

            System.Console.Out.WriteLine("");
            System.Console.Out.WriteLine("User secrets");
            System.Console.Out.WriteLine("--------------");
            System.Console.Out.WriteLine("{");
            System.Console.Out.WriteLine("    \"ConnectionString\": \"Data Source=.;Initial Catalog=...;Integrated Security=true;TrustServerCertificate=True;\"");
            System.Console.Out.WriteLine("}");
            return 0;
        }
        try {
            //System.Diagnostics.Debugger.Launch();
            var configuration = GetConfiguration(args);
            bool isForce = configuration.GetValue<bool>("force");
            bool verbose = configuration.GetValue<bool>("verbose");
            string steps = configuration.GetValue<string>("steps");
            if (string.IsNullOrEmpty(steps)) {
                steps = configuration.GetValue<string>("step");
            }
            if (string.IsNullOrEmpty(steps)) {
                steps = "1,2,3,4,5,6";
            }
            var hsSteps = new HashSet<int>(
                steps
                .Split(',')
                .Select(s => { if (int.TryParse(s, out var i)) { return i; } else { return 0; } })
                .Where(i => i > 0));

            var connectionString = configuration.GetValue<string>("ConnectionString");
            var outputFolder = configuration.GetValue<string>("OutputFolder")
                ?? configuration.GetValue<string>("App:OutputFolder");
            var sqlProjectName = configuration.GetValue<string>("SqlProject");
            var sqlProjectTablesName = configuration.GetValue<string>("SqlProjectTables");
            var sqlProjectDatabaseDevTool = configuration.GetValue<string>("SqlProjectDatabaseDevTool");

            if (string.IsNullOrEmpty(connectionString)) {
                System.Console.Error.WriteLine("ConnectionString is empty");
                return 1;
            }

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
            if (string.IsNullOrEmpty(sqlProjectDatabaseDevTool)) {
                sqlProjectDatabaseDevTool = "Replacement.DatabaseDevTool"; // change this
            }

            if (string.IsNullOrEmpty(outputFolder)) {
                System.Console.Error.WriteLine("outputFolder is empty");
                return 1;
            }


            if (verbose) {

                System.Console.Out.WriteLine($"upperDirectoryPath:{upperDirectoryPath}");
                System.Console.Out.WriteLine($"outputFolder:{outputFolder}");
                System.Console.Out.WriteLine($"connectionString:{connectionString}");
                System.Console.Out.WriteLine($"sqlProjectName:{sqlProjectName}");
                System.Console.Out.WriteLine($"sqlProjectTablesName:{sqlProjectTablesName}");
            }

            // MainGenerateSql
            bool changes = false;

            var dotnetPath = Brimborium.GenerateStoredProcedure.Utility.TryGetDotNetPath();
            if (string.IsNullOrEmpty(dotnetPath)) {
                System.Console.Error.WriteLine("dotnet not found");
                return 1;
            }
            if (conditionRunStep(1, hsSteps)) {
                var sqlProjectTables_csproj = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectTablesName,
                    $"{sqlProjectTablesName}.csproj"
                    );
                var sqlProjectTablesDirectory = System.IO.Path.Combine(
                    upperDirectoryPath,
                    sqlProjectTablesName
                    );

                var subresult = UpdateDatabase(connectionString, dotnetPath, sqlProjectTables_csproj, sqlProjectTablesDirectory, isForce);
                if (subresult != 0) { return subresult; }
            }
#if true
            if (conditionRunStep(2, hsSteps)) {

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
            if (conditionRunStep(3, hsSteps)) {
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
            AddNativeTypeConverter();
            if (conditionRunStep(4, hsSteps)) {
                {
                    var (outputPath, outputNamespace) = Replacement.Contracts.API.PrimaryKeyLocation.GetPrimaryKeyOutputInfo();
                    var subResult = MainGeneratePrimaryKey(connectionString, outputPath, outputNamespace);
                    if (subResult) {

                        System.Console.Out.WriteLine("PrimaryKey modified.");
                        //
                        if (hsSteps.Any()) {
                            System.Console.Out.WriteLine("Trying to rerun.");
                            System.Console.Out.WriteLine("stay tunned");
                            var sqlProject_csproj = System.IO.Path.Combine(
                                upperDirectoryPath,
                                sqlProjectDatabaseDevTool,
                                $"{sqlProjectDatabaseDevTool}.csproj"
                                );
                            var sqlProjectDirectory = System.IO.Path.Combine(
                                upperDirectoryPath,
                                sqlProjectDatabaseDevTool
                                );
                            var psi = new System.Diagnostics.ProcessStartInfo(
                                dotnetPath, $"run \"{sqlProject_csproj}\" -- --steps 5");
                            psi.WorkingDirectory = sqlProjectDirectory;
                            System.Diagnostics.Process.Start(psi);
                            // do not check exit because this prg is already running.
                        } else {
                            System.Console.Out.WriteLine("terminate");
                        }
                        return 0;
                    }
                }
            }
            if (conditionRunStep(5, hsSteps)) {
                var defintions = GetDefintion();

                var (outputPath, outputNamespace, outputClassName) = Replacement.Repository.Service.SqlAccessLocation.GetPrimaryKeyOutputInfo();

                MainGenerateSqlAccess(connectionString, defintions, outputPath, outputNamespace, outputClassName, isForce);
            }
            if (conditionRunStep(6, hsSteps)) {
                // pwsh --file src\Replacement.Contracts\GeneratorConverter.ps1
                {
                    var start = System.DateTime.Now;

                    string fulllNameGeneratorConverter_ps1 = "GeneratorConverter.ps1";
                    string workingDirectory = System.IO.Path.Combine(
                        upperDirectoryPath,
                        @"Replacement.Contracts");

                    System.Console.Out.WriteLine($"fulllNameGeneratorConverter_ps1: {fulllNameGeneratorConverter_ps1}");

                    var psi = new System.Diagnostics.ProcessStartInfo(
                        "pwsh",
                        $"-file \"{fulllNameGeneratorConverter_ps1}\"");
                    psi.WorkingDirectory = workingDirectory;
                    var process = System.Diagnostics.Process.Start(psi);
                    if (process is not null) {
                        process.WaitForExit(30_000);
                        var stop = System.DateTime.Now;
                        System.Console.Out.WriteLine($"{(stop - start).TotalSeconds} sec");
                        if (process.ExitCode == 0) {
                            System.Console.Out.WriteLine($"pwsh -file \"{fulllNameGeneratorConverter_ps1}\" in \"{workingDirectory}\" OK");
                        } else {
                            System.Console.Out.WriteLine($"pwsh -file \"{fulllNameGeneratorConverter_ps1}\" in \"{workingDirectory}\" Failed");
                            return 1;
                        }
                    }
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
    private static bool conditionRunStep(int currentStep, HashSet<int> hsSteps) {
        if (hsSteps.Remove(currentStep)) {
            printSteps(currentStep, hsSteps);
            return true;
        } else {
            return false;
        }
    }
    private static void printSteps(int currentStep, HashSet<int> hsSteps) {
        var steps = new string[] {
            "",
            " 1) Update tables in database.",
            " 2) Generate Sql files.",
            " 3) Update tables & store procedures in database.",
            " 4) Generate PrimaryKey.",
            " 5) Generate SqlAccess C#.",
            " 6) Generate ConvertTo***.",
        };
        System.Console.Out.WriteLine("");
        for (int step = 1; step < steps.Length; step++) {
            if (currentStep == step) {
                System.Console.Out.WriteLine("*" + steps[step]);
            } else if (hsSteps.Contains(step)) {
                System.Console.Out.WriteLine("+" + steps[step]);
            } else {
                System.Console.Out.WriteLine("-" + steps[step]);
            }
        }
    }
    private static int UpdateDatabase(
        string connectionString,
        string dotnetPath,
        string sqlProject_csproj,
        string sqlProjectDirectory,
        bool isForce) {
        var start = System.DateTime.Now;
        var diBin = new System.IO.DirectoryInfo(
            System.IO.Path.Combine(
                sqlProjectDirectory,
                "bin"
                )
            );
        if (!diBin.Exists) {
            diBin.Create();
        }

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
                var stop = System.DateTime.Now;
                System.Console.Out.WriteLine($"{(stop - start).TotalSeconds} sec");
                if (process.ExitCode == 0) {
                    System.Console.Out.WriteLine($"dotnet publish {sqlProject_csproj} OK");
                } else {
                    System.Console.Out.WriteLine($"dotnet publish {sqlProject_csproj} Failed");
                    return 1;
                }
            }
            return 0;
        }

        return 0;
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
        var refTypeAPI = typeof(Replacement.Contracts.API.PrimaryKeyLocation);
        var refTypeEntity = typeof(Replacement.Contracts.Entity.ProjectEntity);
        var refTypeAPINamespace = refTypeAPI.Namespace;
        var refTypeEntityNamespace = refTypeEntity.Namespace;
        var types = refTypeEntity.Assembly.GetTypes()
            .Where(t => (t.Namespace == refTypeAPINamespace) || (t.Namespace == refTypeEntityNamespace))
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
