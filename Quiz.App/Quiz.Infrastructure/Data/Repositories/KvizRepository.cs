using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
using Quiz.Infrastructure.Data.EF;

namespace Quiz.Infrastructure.Data.Repositories;

public class KvizRepository : IKvizRepository
{
    private QuizDbContext _db;

    public KvizRepository(QuizDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public Kviz GetKvizById(int id)
    {
        return _db.Kviz.Include(x => x.Pitanja).FirstOrDefault(x => x.KvizId == id);
    }
}