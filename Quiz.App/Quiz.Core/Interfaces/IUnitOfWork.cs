using System.Threading.Tasks;

namespace Quiz.Core.Interfaces;

public interface IUnitOfWork
{
    Task Commit();
    Task Dispose();
}