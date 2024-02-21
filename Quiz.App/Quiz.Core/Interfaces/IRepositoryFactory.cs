namespace Quiz.Core.Interfaces;

public interface IRepositoryFactory
{
    IRepository<T> Create<T>(IUnitOfWork unitOfWork) where T : class;
}