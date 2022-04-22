global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Configuration;

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

global using Replacement.Contracts.API;
global using Replacement.Repository.Service;
global using Replacement.TestExtensions;

global using Xunit;
global using Xunit.Abstractions;

