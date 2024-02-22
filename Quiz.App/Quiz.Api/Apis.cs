using Microsoft.AspNetCore.Mvc;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Interfaces;
using Quiz.Domain.Services;

namespace Quiz.Api;

[ApiController]
[Route("[controller]")]
public class KvizController : ControllerBase
{
    private readonly IQuizService _iQuizService;

    public KvizController(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
    {
        _iQuizService = new QuizService(unitOfWorkFactory, repositoryFactory);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateKviz(CreateKvizRequest noviKviz)
    {
        try
        {
            var kviz = await _iQuizService.CreateKviz(noviKviz);
            return Ok(kviz);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> EditKviz(EditKvizRequest editKvizRequest)
    {
        try
        {
            var kviz = await _iQuizService.EditKviz(editKvizRequest);
            return Ok(kviz);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("{kvizId}")]
    public async Task<IActionResult> GetKvizById(int kvizId)
    {
        try
        {
            var kviz = await _iQuizService.GetKvizById(kvizId);
            return Ok(kviz);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}