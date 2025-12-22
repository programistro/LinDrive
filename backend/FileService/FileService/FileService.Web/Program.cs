using System.Text.Json.Serialization;
using FileService.Core;
using Minio;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();