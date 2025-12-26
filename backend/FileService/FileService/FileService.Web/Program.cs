using System.Text.Json.Serialization;
using FileService.Core;
using FileService.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddMinio(client => client
    .WithCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
        Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"))
    .WithEndpoint(Environment.GetEnvironmentVariable("AWS_SERVICE_URL"), 9000)
    .WithSSL(false));
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
builder.Services.AddOpenApi();
builder.Services.ConfigureServices();
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
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
builder.Services.AddProblemDetails(c =>
{
    c.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();