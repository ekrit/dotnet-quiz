using System.ComponentModel.DataAnnotations;

namespace Quiz.Core.Models;

public class Quiz
{
    [Key]
    public int QuizId { get; set; }
    public string Name { get; set; }
    public List<Question> Questions { get; set; }
    
    public Quiz()
    {
        Questions = new List<Question>();
    }
}