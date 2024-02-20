using Microsoft.Extensions.Configuration;
using Quiz.Core;
using Quiz.Core.Extensions;

namespace Quiz.Infrastructure;

public class AppSettingsConfigurationProvider : Quiz.Core.Interfaces.IConfigurationProvider
{
    private readonly IConfigurationRoot _configuration;

    public AppSettingsConfigurationProvider()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Config.ContentRoot)
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public string GetString(string settingKey, string defaultValue = null)
    {
        var value = GetSetting(settingKey);
        if (!value.IsNullOrEmpty())
            return value;

        return defaultValue;
    }
    
    private string GetSetting(string settingKey)
    {
        return _configuration[settingKey];
    }
}