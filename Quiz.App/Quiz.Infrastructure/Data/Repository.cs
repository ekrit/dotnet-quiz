using System;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Infrastructure.Data
{
    public class Repository<T> : IRepository<T>
            where T : class
    {
        private QuizDbContext _db;

        public Repository(QuizDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
    }
}
