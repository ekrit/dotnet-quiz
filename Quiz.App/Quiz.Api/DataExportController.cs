using Microsoft.AspNetCore.Mvc;
using Quiz.Core.Interfaces;

namespace Quiz.Api;

[ApiController]
[Route("/quiz/[controller]")]
public class DataExportController : ControllerBase
{
    private readonly IDataExportService _dataExporter;
    private readonly IQuizService _quizService;

    public DataExportController(IDataExportService dataExporter, IQuizService quizService)
    {
        _dataExporter = dataExporter;
        _quizService = quizService;
    }
    
    [HttpGet]
    public async Task<IActionResult> exportData(int id, string format = "csv")
    {
        var data = await _quizService.GetKvizExport(id);

        switch (format)
        {   
            case "csv":
                await _dataExporter.ExportCsv(data, "kviz_data.csv");
                break;
            case "json":
                await _dataExporter.ExportJson(data, "kviz_data.json");
                break;
            case "xml":
                await _dataExporter.ExportXml(data, "kviz_data.xml");
                break;
            default:
                return Content("Format not supported!");
        }

        return Content("Data exported successfully!");
    }
    
}