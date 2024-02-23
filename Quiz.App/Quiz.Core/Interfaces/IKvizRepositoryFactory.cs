namespace Quiz.Core.Interfaces;

public interface IKvizRepositoryFactory
{
    IKvizRepository Create(IUnitOfWork unitOfWork);
}