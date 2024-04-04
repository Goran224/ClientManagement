using Microsoft.Extensions.Configuration;
using System;

public static class AppConfig
{
    private static IConfigurationRoot _configuration;

    static AppConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
    }

    public static string GetConnectionString(string name = "DatabaseConnectionString")
    {
        return _configuration.GetConnectionString(name);
    }
}
