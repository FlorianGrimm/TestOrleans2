# Replacement

cd this-directory

## Build

```cmd
dotnet build
```

## Test

```cmd
dotnet test src\TestOrleans2.WebApp.Test\TestOrleans2.WebApp.Test.csproj
```

## Update SQL

```cmd
dotnet run --project src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj


dotnet run --project .\src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj --steps 1 --force true

dotnet run --project .\src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj --steps 2

dotnet run --project .\src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj --steps 3 --force true

>dotnet run --project .\src\TestOrleans2.DatabaseDevTool\TestOrleans2.DatabaseDevTool.csproj --steps 1,2,3,4,5 --force true
```

## OpenAPI / Swagger
```cmd
dotnet msbuild /t:SwaggerGenerate src\TestOrleans2.WebApp\TestOrleans2.WebApp.csproj
```

```cmd
dotnet watch --project src\TestOrleans2.WebApp.Test\TestOrleans2.WebApp.Test.csproj build
```