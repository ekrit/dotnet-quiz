﻿using Microsoft.EntityFrameworkCore;
using Quiz.Core;
using Quiz.Core.Models;

namespace Quiz.Infrastructure.Data.EF
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Pitanje> Pitanje { get; set; }
        public DbSet<Kviz> Kviz { get; set; }
        public DbSet<RecikliranoPitanje> RecikliranoPitanje { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.DB);
        }
    }
}
