using Quiz.Core;
using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
using Quiz.Infrastructure.Common;

namespace Quiz.Domain.Services;

public class QuizService : DataService, IQuizService
{
    private readonly IQuizRepositoryFactory _quizRepositoryFactory;

    public QuizService(IUnitOfWorkFactory iUnitOfWorkFactory, 
        IRepositoryFactory iRepositoryFactory,
        IQuizRepositoryFactory quizRepositoryFactory) : base(iUnitOfWorkFactory, iRepositoryFactory)
    {
        _quizRepositoryFactory = quizRepositoryFactory;
    }

    public IEnumerable<QuizDto> GetAllQuiz()
    {
        var kvizovi = NewRepository<Core.Models.Quiz>(NewUnitOfWork()).GetAll(x=>x.Questions);

        return kvizovi.Select(kviz => new QuizDto()
            {
                QuizId = kviz.QuizId,
                Name = kviz.Name,
                Questions = kviz.Questions.Select(x => new QuiestionDto()
                    {
                        QuiestionId = x.QuestionId,
                        Text = x.Text,
                        Answer = x.Answer
                    }
                ).ToList()
            }
        ).ToList();
    }

    public QuizDto GetQuizById(int id)
    {
        var kviz = _quizRepositoryFactory.Create(NewUnitOfWork()).GetQuizById(id);
        
        return new QuizDto()
        {
            QuizId = kviz.QuizId,
            Name = kviz.Name,
            Questions = kviz.Questions.Select(x => new QuiestionDto()
                {
                    QuiestionId = x.QuestionId,
                    Text = x.Text,
                    Answer = x.Answer
                }
            ).ToList()
        };
    }

    public IEnumerable<QuiestionDto> GetAllQuestions()
    {
        var pitanja = NewRepository<Question>(NewUnitOfWork()).GetAll();

        return pitanja.Select(data => new QuiestionDto()
            {
                QuiestionId = data.QuestionId,
                Text = data.Text,
                Answer = data.Answer
            }
        ).ToList();
    }

    public IEnumerable<Question> GetAllRecycled()
    {
        return NewRepository<Question>(NewUnitOfWork()).GetAll();
    }

    public async Task<QuizDto> CreateQuiz(CreateQuizRequest noviQuizRequest)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Core.Models.Quiz>(uow);
        var pitanjeRepo = NewRepository<Question>(uow);
        var recikliranaRepo = NewRepository<Question>(uow);

        var recikliranaPitanja = recikliranaRepo.GetAll();

        var noviKviz = new Core.Models.Quiz()
        {
            Name = noviQuizRequest.Text
        };

        await kvizRepo.Add(noviKviz);
        await uow.Commit();

        var novaPitanja = new List<Question>();

        foreach (var pitanje in noviQuizRequest.Questions)
        {
            var existingQuestion = recikliranaPitanja.FirstOrDefault(x => x.Text == pitanje.Text);
            if (existingQuestion != null)
            {
                var novoPitanje = new Question()
                {
                    Text = pitanje.Text,
                    Answer = pitanje.Answer,
                    QuizId = noviKviz.QuizId
                };

                await pitanjeRepo.Add(novoPitanje);
                recikliranaRepo.Delete(existingQuestion);

                novaPitanja.Add(novoPitanje);
            }
            else
            {
                var novoPitanje = new Question()
                {
                    Text = pitanje.Text,
                    Answer = pitanje.Answer,
                    QuizId = noviKviz.QuizId
                };

                await pitanjeRepo.Add(novoPitanje);

                novaPitanja.Add(novoPitanje);
            }
        }

        await uow.Commit();
        noviKviz.Questions = novaPitanja;

        kvizRepo.Update(noviKviz);
        await uow.Commit();

        return new QuizDto()
        {
            QuizId = noviKviz.QuizId,
            Name = noviKviz.Name,
            Questions = noviKviz.Questions.Select(data => new QuiestionDto()
                {
                    QuiestionId = data.QuestionId,
                    Text = data.Text,
                    Answer = data.Answer
                }
            ).ToList()
        };
    }

    public async Task<QuizDto> EditQuiz(EditQuizRequest editQuizRequest)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Core.Models.Quiz>(uow);
        var kvizRepoFactory = _quizRepositoryFactory.Create(uow);
        var pitanjeRepo = NewRepository<Question>(uow);
        
        var existingKviz = kvizRepoFactory.GetQuizById(editQuizRequest.QuizId);

        existingKviz.Name = editQuizRequest.Name;

        foreach (var pitanje in existingKviz.Questions)
        {
            await pitanjeRepo.Add(new Question()
                {
                    Text = pitanje.Text,
                    Answer = pitanje.Answer
                }
            );
        }
        await uow.Commit();
        
        var recikliranaPitanja = pitanjeRepo.GetAll();
        
        var editovanaPitanja = new List<Question>();
        existingKviz.Questions.Clear();
        foreach (var editPitanje in editQuizRequest.Questions)
        {
            var recikliranoPitanje = recikliranaPitanja.FirstOrDefault(x =>
                x.Text == editPitanje.Text &&
                x.Answer == editPitanje.Answer
            );
            
            if (recikliranoPitanje != null)
            {
                editovanaPitanja.Add(new Question()
                    {
                        Text = recikliranoPitanje.Text,
                        Answer = recikliranoPitanje.Answer,
                    }
                ); 

                pitanjeRepo.Delete(recikliranoPitanje);             }
            else
            {
                var novoPitanje = new Question()
                {
                    QuizId = existingKviz.QuizId,
                    Text = editPitanje.Text,
                    Answer = editPitanje.Answer,
                };
                
                await pitanjeRepo.Add(novoPitanje);
                editovanaPitanja.Add(novoPitanje);
            }
        }
        await uow.Commit();

        existingKviz.Questions = editovanaPitanja;
        
        kvizRepo.Update(existingKviz);
        await uow.Commit();

        return new QuizDto()
        {
            QuizId = existingKviz.QuizId,
            Name = existingKviz.Name,
            Questions = existingKviz.Questions.Select(data => new QuiestionDto()
                {
                    QuiestionId = data.QuestionId,
                    Text = data.Text,
                    Answer = data.Answer
                }
            ).ToList()
        };
    }

    public async Task<QuizDto> DeleteQuiz(int Id)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var quizRepo = NewRepository<Core.Models.Quiz>(uow);
        var quizRepoFactory = _quizRepositoryFactory.Create(uow);
        var questionRepo = NewRepository<Question>(uow);

        var kvizToDelete = quizRepoFactory.GetQuizById(Id);

        foreach (var pitanje in kvizToDelete.Questions)
        {
            var novoReciklirano = new Question()
            {
                Text = pitanje.Text,
                Answer = pitanje.Answer
            };

            await questionRepo.Add(novoReciklirano);
            questionRepo.Delete(pitanje);
        }
        await uow.Commit();

        quizRepo.Delete(kvizToDelete);
        await uow.Commit();
        
        return new QuizDto()
        {
            QuizId = kvizToDelete.QuizId,
            Name = kvizToDelete.Name,
            Questions = kvizToDelete.Questions.Select(data => new QuiestionDto()
                {
                    QuiestionId = data.QuestionId,
                    Text = data.Text,
                    Answer = data.Answer
                }
            ).ToList()
        };
    }

    public Task<ExportQuizDto> GetQuizExport(int Id)
    {
        var quiz = _quizRepositoryFactory.Create(NewUnitOfWork()).GetQuizById(Id);

        return Task.FromResult(new ExportQuizDto()
        {
            Name = quiz.Name,
            Questions = quiz.Questions.Select(x=>x.Text).ToList()
        });
    }
    
}
