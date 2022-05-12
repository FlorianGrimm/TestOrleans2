global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Data.Common;
global using System.Linq;
global using System.Text;
global using System.Data;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using Microsoft.Data.SqlClient;

global using Orleans;
global using Orleans.Configuration;
global using Orleans.Providers;
global using Orleans.Runtime;
global using Orleans.Storage;
global using Orleans.Configuration.Overrides;

global using Brimborium.Registrator;
global using Brimborium.Tracking;
global using Brimborium.SqlAccess;

global using TestOrleans2.Contracts.API;
global using TestOrleans2.Contracts.Entity;
global using TestOrleans2.Repository.Service;

global using Xunit;
global using Moq;
global using Moq.Language;


[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]