# Replacement

cd this-directory

## Build

```cmd
dotnet build
```

## Test

```cmd
dotnet test src\Replacement.WebApp.Test\Replacement.WebApp.Test.csproj
```

## Update SQL

```cmd
dotnet run --project src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj


dotnet run --project .\src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj --steps 1 --force true

dotnet run --project .\src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj --steps 2

dotnet run --project .\src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj --steps 3 --force true

>dotnet run --project .\src\Replacement.DatabaseDevTool\Replacement.DatabaseDevTool.csproj --steps 1,2,3,4,5 --force true
```

## OpenAPI / Swagger
```cmd
dotnet msbuild /t:SwaggerGenerate src\Replacement.WebApp\Replacement.WebApp.csproj
```