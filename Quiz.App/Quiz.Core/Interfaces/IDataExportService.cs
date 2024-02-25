using Quiz.Core.Dtos;

namespace Quiz.Core.Interfaces;

public interface IDataExportService
{
    Task ExportCsv(ExportQuizDto data, string fileName);
    Task ExportXml(ExportQuizDto data, string fileName);
    Task ExportJson(ExportQuizDto data, string fileName);
}
