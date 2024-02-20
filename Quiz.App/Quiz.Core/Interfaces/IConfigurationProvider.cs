namespace Quiz.Core.Interfaces;

public interface IConfigurationProvider
{
    string GetString(string settingKey, string defaultValue = null);
}