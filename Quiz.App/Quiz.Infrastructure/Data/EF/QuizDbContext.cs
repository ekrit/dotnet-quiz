using Microsoft.EntityFrameworkCore;
using Quiz.Core;

namespace Quiz.Infrastructure.Data.EF
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions options) : base(options)
        {
        }
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.DB);
        }
    }
}
