using Microsoft.EntityFrameworkCore;
using Quiz.Core;
using Quiz.Core.Models;

namespace Quiz.Infrastructure.Data.EF
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Question> Question { get; set; }
        public DbSet<Core.Models.Quiz> Quiz { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.DB);
        }
    }
}
