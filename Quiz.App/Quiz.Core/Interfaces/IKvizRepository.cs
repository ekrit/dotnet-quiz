using Quiz.Core.Models;

namespace Quiz.Core.Interfaces;

public interface IKvizRepository
{
    Kviz GetKvizById(int id);
}