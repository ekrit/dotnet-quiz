using Microsoft.EntityFrameworkCore;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Infrastructure.Data.Repositories;

public class QuizRepository : IQuizRepository
{
    private QuizDbContext _db;

    public QuizRepository(QuizDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public Core.Models.Quiz GetQuizById(int id)
    {
        return _db.Quiz.Include(x => x.Questions).FirstOrDefault(x => x.QuizId == id);
    }
}