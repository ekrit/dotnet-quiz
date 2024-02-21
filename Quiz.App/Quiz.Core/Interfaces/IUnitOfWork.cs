namespace Quiz.Core.Interfaces;

public interface IUnitOfWork
{
    void Commit();
    void Dispose();
}