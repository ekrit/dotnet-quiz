using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizDbContext>(x => x.UseSqlServer(Config.DB));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
