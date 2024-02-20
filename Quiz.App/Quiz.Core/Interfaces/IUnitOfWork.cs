namespace Quiz.Core.Interfaces;

public interface IUnitOfWork
{
    void Dispose();
    void Commit();
}