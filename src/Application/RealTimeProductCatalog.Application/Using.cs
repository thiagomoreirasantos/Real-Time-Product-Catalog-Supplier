global using System.Net;
global using Microsoft.Extensions.Logging;
global using Polly;
global using Polly.Contrib.WaitAndRetry;
global using Polly.Fallback;
global using Polly.Retry;
global using RealTimeProductCatalog.Application.Interfaces;
global using RealTimeProductCatalog.Infrastructure.Interfaces;
global using FluentValidation.Results;
global using RealTimeProductCatalog.Model.Entities;
global using RealTimeProductCatalog.Application.Validation;
global using System.Text.Json;
global using FluentValidation;
global using RealTimeProductCatalog.Application.Dtos;
global using RealTimeProductCatalog.Model.Enums;