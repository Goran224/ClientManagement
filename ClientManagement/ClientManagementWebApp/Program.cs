using ClientAppCore.Infrastructure.Data;
using ClientAppCore.Shared;
using ClientManagementWebApp.Service.Implementation;
using ClientManagementWebApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
DIServiceFactory.RegisterServices(builder.Services);

//DI Register for MVC Services
builder.Services.AddScoped(typeof(IXmlParsingService), typeof(XmlParsingService));

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
