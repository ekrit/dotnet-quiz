namespace Quiz.Core.Dtos.Requests;

public class CreateQuizRequest
{
    public string Text { get; set; }
    public List<CreateQuestionsRequest> Questions { get; set; }
}

public class CreateQuestionsRequest
{
    public string Text { get; set; }
    public string Answer { get; set; }
}