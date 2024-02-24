using System.Data;
using System.Xml;
using Newtonsoft.Json;
using Quiz.Core;
using Quiz.Core.Dtos;
using Quiz.Core.Interfaces;
using Formatting = Newtonsoft.Json.Formatting;

namespace Quiz.Domain.Services;

public class DataExporterService : IDataExportService
{
    public Task ExportCsv(ExportKvizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = new StreamWriter(filePath);
    
        writer.WriteLine("NazivKviza,Pitanje");
    
        foreach (var question in data.Pitanja)
        {
            writer.WriteLine($"{data.Naziv},{question}");
        }
    
        return Task.CompletedTask;
    }

    public Task ExportXml(ExportKvizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true });

        writer.WriteStartElement("Kviz");
    
        writer.WriteElementString("NazivKviza", data.Naziv);
    
        writer.WriteStartElement("Pitanje");
        foreach (var question in data.Pitanja)
        {
            writer.WriteElementString("Pitanje", question);
        }
        writer.WriteEndElement();
    
        writer.WriteEndElement();
    
        return Task.CompletedTask;
    }

    public Task ExportJson(ExportKvizDto data, string fileName)
    {
        var filePath = Path.Combine(Config.ExportDirectory, fileName);

        using var writer = new StreamWriter(filePath);
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        writer.Write(json);
        return Task.CompletedTask;
    }

}