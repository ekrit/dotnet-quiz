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
        var data = NewRepository<Question>(NewUnitOfWork()).Get(x=>x.IsRecycled != true);

        return data.Select(x => new QuiestionDto()
            {
                QuiestionId = x.QuestionId,
                Text = x.Text,
                Answer = x.Answer
            }
        ).ToList();
    }

    public IEnumerable<RecycledQuestionDto> GetAllRecycled()
    {
        var data = NewRepository<Question>(NewUnitOfWork()).Get(x => x.IsRecycled == true);
        
        return data.Select(x => new RecycledQuestionDto()
            {
                QuiestionId = x.QuestionId,
                Text = x.Text,
                Answer = x.Answer
            }
        ).ToList();
    }

    public async Task<QuizDto> CreateQuiz(CreateQuizRequest newQuizRequest)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var quizRepo = NewRepository<Core.Models.Quiz>(uow);
        var questionRepo = NewRepository<Question>(uow);

        var recycledQuestions = questionRepo.Get(x=>x.IsRecycled == true);

        var newQuiz = new Core.Models.Quiz()
        {
            Name = newQuizRequest.Text
        };

        await quizRepo.Add(newQuiz);
        await uow.Commit();

        var editedQuestions = new List<Question>();
        
        foreach (var q in newQuizRequest.Questions)
        {
            var existingQuestion = recycledQuestions.FirstOrDefault(x => x.Text == q.Text && x.Answer == q.Answer);
            if (existingQuestion != null)
            {
                var newQuestion = new Question()
                {
                    Text = q.Text,
                    Answer = q.Answer,
                    QuizId = newQuiz.QuizId
                };

                await questionRepo.Add(newQuestion);
                questionRepo.Delete(existingQuestion);

                editedQuestions.Add(newQuestion);
            }
            else
            {
                var newQuestion = new Question()
                {
                    Text = q.Text,
                    Answer = q.Answer,
                    QuizId = newQuiz.QuizId
                };

                await questionRepo.Add(newQuestion);

                editedQuestions.Add(newQuestion);
            }
        }

        await uow.Commit();
        
        newQuiz.Questions = editedQuestions;
        quizRepo.Update(newQuiz);
        await uow.Commit();

        return new QuizDto()
        {
            QuizId = newQuiz.QuizId,
            Name = newQuiz.Name,
            Questions = newQuiz.Questions.Select(data => new QuiestionDto()
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
        var quizRepo = NewRepository<Core.Models.Quiz>(uow);
        var quizRepoFactory = _quizRepositoryFactory.Create(uow);
        var questionRepo = NewRepository<Question>(uow);
        
        var existingQuiz = quizRepoFactory.GetQuizById(editQuizRequest.QuizId);

        existingQuiz.Name = editQuizRequest.Name;

        await questionRepo.Add(existingQuiz.Questions.Select(q => new Question()
                {
                    Text = q.Text,
                    Answer = q.Answer,
                    IsRecycled = true
                }
            ).ToList()
        );
        
        await uow.Commit();
        
        var recycledQuestions = questionRepo.Get(x=>x.IsRecycled == true);
        
        var editedQuestions = new List<Question>();
        existingQuiz.Questions.Clear();
        foreach (var q in editQuizRequest.Questions)
        {
            var recycledQuestion = recycledQuestions.FirstOrDefault(x =>
                x.Text == q.Text &&
                x.Answer == q.Answer
            );
            
            if (recycledQuestion != null)
            {
                editedQuestions.Add(new Question()
                    {
                        QuizId = existingQuiz.QuizId,
                        Text = recycledQuestion.Text,
                        Answer = recycledQuestion.Answer,
                    }
                ); 

                questionRepo.Delete(recycledQuestion);             }
            else
            {
                var newQuestion = new Question()
                {
                    QuizId = existingQuiz.QuizId,
                    Text = q.Text,
                    Answer = q.Answer,
                };
                
                await questionRepo.Add(newQuestion);
                editedQuestions.Add(newQuestion);
            }
        }
        await uow.Commit();

        existingQuiz.Questions = editedQuestions;

        quizRepo.Update(existingQuiz);
        await uow.Commit();

        return new QuizDto()
        {
            QuizId = existingQuiz.QuizId,
            Name = existingQuiz.Name,
            Questions = existingQuiz.Questions.Select(data => new QuiestionDto()
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

        var quizToDelete = quizRepoFactory.GetQuizById(Id);

        foreach (var q in quizToDelete.Questions)
        {
            var newRecycled = new Question()
            {
                Text = q.Text,
                Answer = q.Answer,
                IsRecycled = true
            };

            await questionRepo.Add(newRecycled);
            questionRepo.Delete(q);
        }
        await uow.Commit();

        quizRepo.Delete(quizToDelete);
        await uow.Commit();
        
        return new QuizDto()
        {
            QuizId = quizToDelete.QuizId,
            Name = quizToDelete.Name,
            Questions = quizToDelete.Questions.Select(data => new QuiestionDto()

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
