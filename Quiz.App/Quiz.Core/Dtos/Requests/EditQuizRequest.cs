namespace Quiz.Core.Dtos.Requests;

public class EditQuizRequest
{
    public int QuizId { get; set; }
    public string Name { get; set; }
    public List<EditQuestionRequest> Questions { get; set; }
}

public class EditQuestionRequest
{
    public string Text { get; set; }
    public string Answer { get; set; }
}