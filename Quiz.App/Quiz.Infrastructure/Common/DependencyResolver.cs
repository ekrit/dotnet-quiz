namespace Quiz.Infrastructure.Common;

public static class DependencyResolver
{
    public static Core.Interfaces.IConfigurationProvider GetConfigurationProvider()
    {
        return new AppSettingsConfigurationProvider();
    }
}
