using Microsoft.EntityFrameworkCore;
using Quiz.Core.Models;

namespace Quiz.Infrastructure.Data.EF
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Bezveze> Bezveze { get; set; }
    }
}
