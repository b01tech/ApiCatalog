using ApiCatalog.Data;
using ApiCatalog.Logging;
using ApiCatalog.Repositories;
using ApiCatalog.Repositories.Interfaces;
using ApiCatalog.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var mySqlConnection = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped<SeedingDbService>();
builder.Services.AddScoped<LoggingFilter>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Logging.AddProvider(new LoggingProvider(new LoggingConfiguration { LogLevel = LogLevel.Warning}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

using (var scope = app.Services.CreateScope())
{
    var seedingService = scope.ServiceProvider.GetRequiredService<SeedingDbService>();
    seedingService.SeedDb();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();

