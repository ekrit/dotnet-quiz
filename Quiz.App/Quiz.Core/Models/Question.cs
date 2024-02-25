using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Core.Models;

public class Question
{
    [Key]
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public string Answer { get; set; }
    [ForeignKey(nameof(quiz))]
    public int? QuizId { get; set; }
    public Quiz quiz { get; set; }
    public bool IsRecycled { get; set; } = false;

}