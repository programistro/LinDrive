using System.Text.Json.Serialization;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Services;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Application.Services.IO.Services;
using LinDrive.Core;
using LinDrive.Core.Interfaces;
using LinDrive.Infrastructure;
using LinDrive.Infrastructure.Data;
using LinDrive.Infrastructure.Repositories;
using LinDrive.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

foreach (var kvp in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started");

builder.Services.Configure<S3Options>(option =>
{
    option.AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
    option.Bucket = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");
    option.SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
    option.ServiceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");
});
builder.Services.AddMinio(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
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
