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
        public DbSet<Bezveze> Bezveze { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.DB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
