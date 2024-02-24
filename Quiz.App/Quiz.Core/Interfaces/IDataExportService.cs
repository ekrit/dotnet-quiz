using System.Data;
using System.Threading.Tasks;
using Quiz.Core.Dtos;

namespace Quiz.Core.Interfaces;

public interface IDataExportService
{
    Task ExportCsv(ExportKvizDto data, string fileName);
    Task ExportXml(ExportKvizDto data, string fileName);
    Task ExportJson(ExportKvizDto data, string fileName);
}
