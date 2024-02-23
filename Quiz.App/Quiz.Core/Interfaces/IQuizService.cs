using System.Collections.Generic;
using System.Threading.Tasks;
using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Models;

namespace Quiz.Core.Interfaces;

public interface IQuizService
{
    IEnumerable<KvizDto> GetAllKvizovi();
    KvizDto GetKvizById(int id);
    IEnumerable<RecikliranoPitanje> GetAllReciklirana();
    IEnumerable<PitanjeDto> GetAllPitanja();
    Task<KvizDto> CreateKviz(CreateKvizRequest noviKviz);
    Task<KvizDto> EditKviz(EditKvizRequest editKvizRequest);
    Task<KvizDto> DeleteKviz(int Id);
    Task<ExportKvizDto> GetKvizExport(int Id);
}