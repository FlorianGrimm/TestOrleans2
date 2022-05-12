global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Diagnostics.Tracing;
global using System.Runtime.CompilerServices;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore;
global using Microsoft.AspNetCore.Authentication.Negotiate;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.OData;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Hosting;

global using Microsoft.OData.Edm;
global using Microsoft.OData.Edm.Csdl;
global using Microsoft.OData.Edm.Validation;
global using Microsoft.OData.ModelBuilder;

global using Microsoft.OpenApi.Models;

global using Polly;
global using Polly.Retry;

global using Orleans;
global using Orleans.Hosting;

global using Brimborium.Registrator;

global using Replacement.Contracts.API;
global using Replacement.Contracts.Entity;

global using Replacement.Repository.Grains;
global using Replacement.Repository.Service;
global using Replacement.Repository.Extensions;

global using Replacement.WebApp.Swagger;
global using Replacement.WebApp.Services;
global using Replacement.WebApp.Controllers;
global using Replacement.WebApp.Extensions;
