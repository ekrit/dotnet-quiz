using System.Xml;
using Newtonsoft.Json;
using Quiz.Core;
using Quiz.Core.Dtos;
using Quiz.Core.Interfaces;
using Formatting = Newtonsoft.Json.Formatting;

namespace Quiz.Domain.Services;

public class DataExporterService : IDataExportService
{
    public Task ExportCsv(ExportQuizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = new StreamWriter(filePath);
    
        writer.WriteLine("NazivKviza,Pitanje");
    
        foreach (var question in data.Questions)
        {
            writer.WriteLine($"{data.Name},{question}");
        }
    
        return Task.CompletedTask;
    }

    public Task ExportXml(ExportQuizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true });

        writer.WriteStartElement("Kviz");
    
        writer.WriteElementString("NazivKviza", data.Name);
    
        writer.WriteStartElement("Pitanje");
        foreach (var question in data.Questions)
        {
            writer.WriteElementString("Pitanje", question);
        }
        writer.WriteEndElement();
    
        writer.WriteEndElement();
    
        return Task.CompletedTask;
    }

    public Task ExportJson(ExportQuizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = new StreamWriter(filePath);
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        writer.Write((string?)json);
        return Task.CompletedTask;
    }

}