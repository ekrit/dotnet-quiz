using Quiz.Core;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly Enums.UnitOfWorkMode _mode;
    internal QuizDbContext Db { get; }

    public UnitOfWork(QuizDbContext db, Enums.UnitOfWorkMode mode)
    {
        Db = db ?? throw new ArgumentNullException(nameof(db));
        _mode = mode;
    }
    
    public async Task Commit()
    {
        if (_mode == Enums.UnitOfWorkMode.ReadOnly)
            throw new InvalidOperationException("Commit is not allowed in read-only UnitOfWork.");

        await Db.SaveChangesAsync();
    }
    
    public async Task Dispose()
    {
        await Db.DisposeAsync();
    }
}