using System.Text.Json.Serialization;
using LinDrive.Core;
using LinDrive.Infrastructure;
using LinDrive.Infrastructure.Data;
using LinDrive.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Scalar.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

foreach (var kvp in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
builder
    .Services.AddOpenApi();

builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureValidatos();
builder.Services.AddProblemDetails(c =>
{
    c.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});
builder.Services.AddFluentValidationAutoValidation(config =>
{
    // Disable the built-in .NET model (data annotations) validation.
    config.DisableBuiltInModelValidation = true;

    // Only validate controllers decorated with the `AutoValidation` attribute.
    config.ValidationStrategy = ValidationStrategy.All;

    // Enable validation for parameters bound from `BindingSource.Body` binding sources.
    config.EnableBodyBindingSourceAutomaticValidation = true;

    // Enable validation for parameters bound from `BindingSource.Form` binding sources.
    config.EnableFormBindingSourceAutomaticValidation = true;

    // Enable validation for parameters bound from `BindingSource.Query` binding sources.
    config.EnableQueryBindingSourceAutomaticValidation = true;

    // Enable validation for parameters bound from `BindingSource.Path` binding sources.
    config.EnablePathBindingSourceAutomaticValidation = true;

    // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
    config.EnableCustomBindingSourceAutomaticValidation = true;
});
var app = builder.Build();

app.UseRouting();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapControllers();
app.Run();
