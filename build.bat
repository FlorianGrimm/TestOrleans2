cd %~dp0
dotnet restore

dotnet run --project src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj
@REM dotnet run --project src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj -- --force

dotnet build

dotnet build src\Replacement.WebApp\Replacement.WebApp.csproj
dotnet msbuild /t:SwaggerGenerate src\Replacement.WebApp\Replacement.WebApp.csproj

dotnet build src\Replacement.Client\Replacement.Client.csproj
dotnet msbuild /t:SwaggerGenerate src\Replacement.Client\Replacement.Client.csproj

dotnet build
