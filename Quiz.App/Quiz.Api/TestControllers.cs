using Microsoft.AspNetCore.Mvc;
using Quiz.Core;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure.Common;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Api;

[ApiController]
[Route("[controller]/[action]")]
public class TestControllers :  DataService
{
    private readonly QuizDbContext _dbContext;

    public TestControllers(QuizDbContext dbContext,
        IUnitOfWorkFactory unitOfWorkFactory,
        IRepositoryFactory repositoryFactory) : base(unitOfWorkFactory,repositoryFactory)
    {
        _dbContext = dbContext;
    }

}