
using ArmEncoder.Sample.Tags;
using StdUnit.Tags.WebUi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

app.MapTagsStream();
app.Run();