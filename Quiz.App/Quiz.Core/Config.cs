using Quiz.Core.Interfaces;

namespace Quiz.Core;

public class Config
{
    private static IConfigurationProvider _provider;
    
    public static void SetConfigurationProvider(IConfigurationProvider provider)
    {
        _provider = provider;
    }
    
    public static string DB => _provider.GetString("DB");
    public static string ExportDirectory => _provider.GetString("ExportDirectory");
    
    public static string ContentRoot => Directory.GetCurrentDirectory();

}