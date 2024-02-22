namespace Quiz.Infrastructure.Common;

public class DependencyResolver
{
    public static Core.Interfaces.IConfigurationProvider GetConfigurationProvider()
    {
        return new AppSettingsConfigurationProvider();
    }
}
