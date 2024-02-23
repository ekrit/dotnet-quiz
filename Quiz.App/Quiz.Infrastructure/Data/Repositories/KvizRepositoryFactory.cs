using Quiz.Core.Interfaces;

namespace Quiz.Infrastructure.Data.Repositories;

public class KvizRepositoryFactory : IKvizRepositoryFactory
{
    public IKvizRepository Create(IUnitOfWork unitOfWork)
    {
        return new KvizRepository((unitOfWork as UnitOfWork).Db);
    }
}