using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Core;
using Quiz.Infrastructure.Data.EF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizDbContext>(x => x.UseSqlServer(Config.DB));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
