using Microsoft.AspNetCore.Mvc;
using Quiz.Core;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
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

    [HttpPost]
    public async Task<Bezveze> Add(Bezveze nesto)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var repo = NewRepository<Bezveze>(uow);

        await repo.Add(nesto);
        uow.Commit();
        
        return nesto;
    }
    
    [HttpGet]
    public Task<IEnumerable<Bezveze>> GetAll()
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var repo = NewRepository<Bezveze>(uow);

        var res = repo.GetAll();
        
        return Task.FromResult(res);
    }

    [HttpDelete]
    public void Delete(Bezveze nesto)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var repo = NewRepository<Bezveze>(uow);
        
        repo.Delete(nesto);
        uow.Commit();
    }

    [HttpPut]
    public Task<Bezveze> Update(Bezveze nesto)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var repo = NewRepository<Bezveze>(uow);
        
        repo.Update(nesto);
        uow.Commit();
        
        return Task.FromResult(nesto);
    }
}