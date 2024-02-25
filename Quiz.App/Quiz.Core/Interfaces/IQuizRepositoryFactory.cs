namespace Quiz.Core.Interfaces;

public interface IQuizRepositoryFactory
{
    IQuizRepository Create(IUnitOfWork unitOfWork);
}