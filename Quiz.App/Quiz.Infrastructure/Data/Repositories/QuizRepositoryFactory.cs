using Quiz.Core.Interfaces;

namespace Quiz.Infrastructure.Data.Repositories;

public class QuizRepositoryFactory : IQuizRepositoryFactory
{
    public IQuizRepository Create(IUnitOfWork unitOfWork)
    {
        return new QuizRepository((unitOfWork as UnitOfWork).Db);
    }
}