using Microsoft.EntityFrameworkCore;
using Quiz.Core;
using Quiz.Core.Interfaces;
using Quiz.Infrastructure;
using Quiz.Infrastructure.Data;
using Quiz.Infrastructure.Data.EF;

var builder = WebApplication.CreateBuilder(args);

Config.SetConfigurationProvider(DependencyResolver.GetConfigurationProvider());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddDbContext<QuizDbContext>(x => x.UseSqlServer(Config.DB));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
