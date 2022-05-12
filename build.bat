cd %~dp0
dotnet restore

dotnet run --project src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj
@REM dotnet run --project src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj -- --force

dotnet build

dotnet build src\TestOrleans2.WebApp\TestOrleans2.WebApp.csproj
dotnet msbuild /t:SwaggerGenerate src\TestOrleans2.WebApp\TestOrleans2.WebApp.csproj

dotnet build src\TestOrleans2.Client\TestOrleans2.Client.csproj
dotnet msbuild /t:SwaggerGenerate src\TestOrleans2.Client\TestOrleans2.Client.csproj

dotnet build
