using System.Collections.Generic;
using System.Threading.Tasks;
using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Models;

namespace Quiz.Core.Interfaces;

public interface IQuizService
{
    IEnumerable<Kviz> GetAllKvizovi();
    Task<Kviz> GetKvizById(int id);
    IEnumerable<RecikliranoPitanje> GetAllReciklirana();
    IEnumerable<Pitanje> GetAllPitanja();
    Task<Kviz> CreateKviz(CreateKvizRequest noviKviz);
    Task<Kviz> EditKviz(EditKvizRequest editKvizRequest);
    Task<Kviz> DeleteKviz(int Id);
    Task<ExportKvizDto> GetKvizExport(int Id);
}