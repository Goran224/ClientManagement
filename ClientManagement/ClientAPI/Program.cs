using ClientAppCore.Infrastructure.Data;
using ClientAppCore.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
DIServiceFactory.RegisterServices(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString = AppConfig.GetConnectionString("DatabaseConnectionString");

builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(dbConnectionString));

//Serilog Config
Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log.txt",
                          rollingInterval: RollingInterval.Day,
                          restrictedToMinimumLevel: LogEventLevel.Error)
                          .MinimumLevel.Error()
            .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
