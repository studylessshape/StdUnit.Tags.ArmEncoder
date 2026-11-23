
using StdUnit.Tags.WebUi;
using ArmEncoder.Sample.Tags;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();

builder.Services.AddTags();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

app.MapTagsStream();
app.Run();