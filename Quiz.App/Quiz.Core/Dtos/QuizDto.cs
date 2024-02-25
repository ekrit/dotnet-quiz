namespace Quiz.Core.Dtos;

public class QuizDto
{
        public int QuizId { get; set; }
        public string Name { get; set; }
        public List<QuiestionDto> Questions { get; set; }
}

public class QuiestionDto
{
    public int QuiestionId { get; set; }
    public string Text { get; set; }
    public string Answer { get; set; }
}
