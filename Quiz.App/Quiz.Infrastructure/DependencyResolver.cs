namespace Quiz.Infrastructure;

public class DependencyResolver
{
    public static Core.Interfaces.IConfigurationProvider GetConfigurationProvider()
    {
        return new AppSettingsConfigurationProvider();
    }
}
