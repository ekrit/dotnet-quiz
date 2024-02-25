using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz.Core;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Interfaces;
using Quiz.Domain.Services;
using Quiz.Infrastructure.Common;
using Quiz.Infrastructure.Data;
using Quiz.Infrastructure.Data.EF;
using Quiz.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

Config.SetConfigurationProvider(DependencyResolver.GetConfigurationProvider());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<IQuizRepositoryFactory, QuizRepositoryFactory>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IDataExportService, DataExporterService>();

builder.Services.AddControllers().AddNewtonsoftJson(x => 
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<QuizDbContext>(x => x.UseSqlServer(Config.DB));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/quiz/getAll", (IQuizService quizService) => quizService.GetAllQuiz());
app.MapGet("/quiz/getAllPitanja", (IQuizService quizService) => quizService.GetAllQuestions());
app.MapGet("/quiz/getAllReciklirana", (IQuizService quizService) => quizService.GetAllRecycled());
app.MapGet("/quiz/getKvizById/{id}", (int id,IQuizService quizService) => quizService.GetQuizById(id));
app.MapPost("/quiz/create", async ([FromBody] CreateQuizRequest noviKviz, IQuizService quizService) => await quizService.CreateQuiz(noviKviz));
app.MapPut("/quiz/edit", async ([FromBody] EditQuizRequest edit, IQuizService quizService) => await quizService.EditQuiz(edit));
app.MapDelete("/quiz/deleteQuiz", (int id,IQuizService quizService) => quizService.DeleteQuiz(id));


app.UseAuthorization();

app.MapControllers();

app.Run();
