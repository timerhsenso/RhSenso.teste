// --------------------------------------------------------------------------------------
//  RhSenso API - Program.cs
//  Configuração completa da API REST com JWT, CORS, Swagger, Multi-tenancy e Auditoria
// --------------------------------------------------------------------------------------

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RhS.Api.Middleware;
using RhS.Api.Swagger;
using RhS.SEG.Application.Interfaces.Botoes;
using RhS.SEG.Application.Interfaces.Sistemas;
using RhS.SEG.Application.Interfaces.Usuarios;
using RhS.SharedKernel.Exceptions;
using RhS.SharedKernel.Interfaces;
using RhS.SEG.Application.Services;
using RhS.Infrastructure;
using RhS.Infrastructure.Data;
using RhS.Infrastructure.Services;
using RhS.Infrastructure.Services.SEG.Botoes;
using RhS.Infrastructure.Services.SEG.Sistemas;
using RhS.Infrastructure.Services.SEG.Usuarios;
using RhS.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// SERILOG — logging estruturado
// ============================================================================
builder.Host.UseSerilog((context, config) =>
    config
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/rhsenso-api-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30));

// ============================================================================
// SERVICES (DI)
// ============================================================================

// Controllers + JSON
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation(opt =>
{
    opt.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<RhS.Api.Validators.SEG.BotaoFormValidator>();

// API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version")
    );
});

// Database
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<RhSensoDbContext>(options =>
    options.UseSqlServer(connectionString));

// Multi-tenancy
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantAccessor, TenantAccessor>();

// Authentication & Authorization
var jwtSection = builder.Configuration.GetSection("JWT");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not found.");
var jwtIssuer = jwtSection["Issuer"] ?? "RhSenso";
var jwtAudience = jwtSection["Audience"] ?? "RhSenso-Clients";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// CORS
var corsSection = builder.Configuration.GetSection("Cors");
var allowedOrigins = corsSection.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontends", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins);
        }
        else
        {
            policy.AllowAnyOrigin();
        }
        policy.AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RhSenso API",
        Version = "v1",
        Description = "API REST do Sistema RhSenso"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RhSensoDbContext>();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICacheService, CacheService>();

// SEG Services
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<ISistemasService, SistemasService>();
builder.Services.AddScoped<IBotoesService, BotoesService>();

// Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Cache
builder.Services.AddMemoryCache();

// Custom response for validation errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage)
            .ToArray();

        var response = new BaseResponse
        {
            Success = false,
            Message = "Validation failed",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});

var app = builder.Build();

// ============================================================================
// MIDDLEWARE PIPELINE
// ============================================================================

// Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RhSenso API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

// Security
app.UseHttpsRedirection();
app.UseCors("Frontends");

// Custom Middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration.TotalMilliseconds
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

// Controllers
app.MapControllers();

// Root endpoint
app.MapGet("/", () => new { 
    service = "RhSenso API", 
    version = "1.0", 
    status = "running",
    timestamp = DateTime.UtcNow 
});

app.Run();

