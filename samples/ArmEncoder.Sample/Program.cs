
using ArmEncoder.Sample.Tags;
using StdUnit.Tags.WebUi;
using System.Text.Json.Serialization;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Debug(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog(opts => opts.ReadFrom.Configuration(builder.Configuration));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAntiforgery();

    builder.Services.AddControllers()
        .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    builder.Services.AddSwaggerGen(o => o.UseInlineDefinitionsForEnums());

    builder.Services.AddTags();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.MapControllers();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAntiforgery();

    app.MapTagsStream();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
