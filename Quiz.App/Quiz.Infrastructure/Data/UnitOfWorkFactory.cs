using Microsoft.EntityFrameworkCore;
using Quiz.Core;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure.Data.EF;


namespace Quiz.Infrastructure.Data;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    public IUnitOfWork Create(Enums.UnitOfWorkMode mode = Enums.UnitOfWorkMode.ReadOnly)
    {
        var options = new DbContextOptionsBuilder()
#if DEBUG
            .EnableSensitiveDataLogging()
#endif
            .UseSqlServer(Config.DB)
            .Options;

        var db = new QuizDbContext(options);

        db.ChangeTracker.LazyLoadingEnabled = false;

        if (mode == Enums.UnitOfWorkMode.ReadOnly)
        {
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            //db.Configuration.ProxyCreationEnabled = false;
        }

        return new UnitOfWork(db, mode);
    }
}