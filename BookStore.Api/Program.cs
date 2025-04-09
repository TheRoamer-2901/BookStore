using System.Text.RegularExpressions;
using BookStore.Api.Middlewares;
using BookStore.Common;
using BookStore.Contract;
using BookStore.Persistence;
using BookStore.Service;

var builder = WebApplication.CreateBuilder(args);

// Add configs
var configPath = Regex.Replace(AppContext.BaseDirectory, "(Console|Api)", "Common");
builder.Configuration
    .SetBasePath(configPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 

// Register all layers
builder.Services
    .RegisterCommonServices()
    .RegisterServiceLayer()
    .RegisterPersistenceLayer(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBookStoreMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();