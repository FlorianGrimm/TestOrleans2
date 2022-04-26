cd %~dp0
dotnet restore

dotnet run --project src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj

dotnet build

dotnet msbuild /t:SwaggerGenerate src\Replacement.WebApp\Replacement.WebApp.csproj

dotnet msbuild /t:SwaggerGenerate src\Replacement.Client\Replacement.Client.csproj
