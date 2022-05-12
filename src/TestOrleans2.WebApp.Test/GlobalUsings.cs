global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Threading.Tasks;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Configuration;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Extensions.Hosting;

global using Orleans;
global using Orleans.Configuration;
global using Orleans.Providers;
global using Orleans.Runtime;
global using Orleans.Internal;
global using Orleans.Storage;
global using Orleans.Hosting;
global using Orleans.TestingHost;
global using Orleans.Configuration.Overrides;

global using Brimborium.Registrator;

global using TestOrleans2.Contracts.API;
global using TestOrleans2.Contracts.Entity;

global using TestOrleans2.Repository.Grains;
global using TestOrleans2.Repository.Service;
global using TestOrleans2.Repository.Extensions;

global using TestOrleans2.WebApp.Swagger;
global using TestOrleans2.WebApp.Services;
global using TestOrleans2.WebApp.Controllers;
global using TestOrleans2.WebApp.Extensions;

global using TestOrleans2.TestExtensions;

global using Xunit;
global using Xunit.Abstractions;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]