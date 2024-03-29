﻿using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Models;

namespace Quiz.Core.Interfaces;

public interface IQuizService
{
    IEnumerable<QuizDto> GetAllQuiz();
    QuizDto GetQuizById(int id);
    IEnumerable<RecycledQuestionDto> GetAllRecycled();
    IEnumerable<QuiestionDto> GetAllQuestions();
    Task<QuizDto> CreateQuiz(CreateQuizRequest newQuizRequest);
    Task<QuizDto> EditQuiz(EditQuizRequest editQuizRequest);
    Task<QuizDto> DeleteQuiz(int Id);
    Task<ExportQuizDto> GetQuizExport(int Id);
}