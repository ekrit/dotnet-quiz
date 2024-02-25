namespace Quiz.Core.Interfaces;

public interface IQuizRepository
{
    Models.Quiz GetQuizById(int id);
}