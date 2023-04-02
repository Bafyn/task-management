using TaskManagement.API;
using TaskManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRequestHandlingServices();
builder.Services.AddApplicationServices();
builder.Services.AddPersistence(configuration);
builder.Services.AddServiceBus(configuration);
builder.Services.AddMessageConsumers();

var app = builder.Build();

// Apply DB migrations at run-time to avoid using external tools.
await app.Services.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
