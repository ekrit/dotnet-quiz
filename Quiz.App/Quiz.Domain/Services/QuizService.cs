using Quiz.Core;
using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
using Quiz.Infrastructure.Common;

namespace Quiz.Domain.Services;

public class QuizService : DataService, IQuizService
{
    public QuizService(IUnitOfWorkFactory iUnitOfWorkFactory, IRepositoryFactory iRepositoryFactory) : base(iUnitOfWorkFactory, iRepositoryFactory)
    {
    }

    public IEnumerable<Kviz> GetAllKvizovi()
    {
        var uow = NewUnitOfWork();
        return NewRepository<Kviz>(uow).GetAll();
    }
    
    public async Task<Kviz> GetKvizById(int id) => await NewRepository<Kviz>(NewUnitOfWork()).GetById(id);

    public IEnumerable<Pitanje> GetAllPitanja()
    {
        var uow = NewUnitOfWork();
        return NewRepository<Pitanje>(uow).GetAll();
    }
    
    public IEnumerable<RecikliranoPitanje> GetAllReciklirana()
    {
        var uow = NewUnitOfWork();
        return NewRepository<RecikliranoPitanje>(uow).GetAll();
    }

    public async Task<Kviz> CreateKviz(CreateKvizRequest noviKviz)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Kviz>(uow);
        var pitanjeRepo = NewRepository<Pitanje>(uow);
        var recikliranaRepo = NewRepository<RecikliranoPitanje>(uow);

        var recikliranaPitanja = recikliranaRepo.GetAll();

        var newKviz = new Kviz()
        {
            Naziv = noviKviz.Naziv
        };

        await kvizRepo.Add(newKviz);

        var novaPitanja = new List<Pitanje>();

        foreach (var pitanje in noviKviz.Pitanja)
        {
            var existingQuestion = recikliranaPitanja.FirstOrDefault(x => x.Sadrzaj == pitanje.Sadrzaj);
            if (existingQuestion != null)
            {
                // Ako pitanje ne postoji u recikliranim pitanjima, potrebno je kreirati novo pitanje te ga dodati u bazu
                // i spremiti u listu koja će kasnije biti uvrštena u kviz
                
                var novoPitanje = new Pitanje()
                {
                    Sadrzaj = pitanje.Sadrzaj,
                    Odgovor = pitanje.Odgovor,
                    KvizId = newKviz.KvizId
                };

                await pitanjeRepo.Add(novoPitanje);
                
                novaPitanja.Add(novoPitanje);
            }
            else
            {
                // Ako pitanje postoji među recikliranim pitanjima, potrebno ga je premjestiti u tabelu pitanje te ga dodati u listu
                // također izbrisati ga iz tabele reciklirana pitanja
                
                var novoPitanje = new Pitanje()
                {
                    Sadrzaj = existingQuestion.Sadrzaj,
                    Odgovor = existingQuestion.Odgovor,
                    KvizId = newKviz.KvizId
                };

                await pitanjeRepo.Add(novoPitanje);
                recikliranaRepo.Delete(existingQuestion);
                
                novaPitanja.Add(novoPitanje);
            }
        }

        newKviz.Pitanja = novaPitanja;

        kvizRepo.Update(newKviz);
        uow.Commit();

        return newKviz;
    }

    public async Task<Kviz> EditKviz(EditKvizRequest editKvizRequest)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Kviz>(uow);
        var pitanjeRepo = NewRepository<Pitanje>(uow);

        var allPitanja = pitanjeRepo.GetAll();

        var existingKviz = await kvizRepo.GetById(editKvizRequest.KvizId);

        existingKviz.Naziv = editKvizRequest.Naziv;

        existingKviz.Pitanja.Clear();
        foreach (var editPitanje in editKvizRequest.Pitanja)
        {
            var dbQuestion = allPitanja.FirstOrDefault(q => q.PitanjeId == editPitanje.PitanjeId);
            if (dbQuestion != null)
            {
                dbQuestion.Sadrzaj = editPitanje.Sadrzaj;
                dbQuestion.Odgovor = editPitanje.Odgovor;
                
                pitanjeRepo.Update(dbQuestion);
            }
            else
            {
                dbQuestion = new Pitanje { PitanjeId = editPitanje.PitanjeId, Sadrzaj = editPitanje.Sadrzaj, Odgovor = editPitanje.Odgovor};
                await pitanjeRepo.Add(dbQuestion);
            }
            existingKviz.Pitanja.Add(dbQuestion);
        }

        kvizRepo.Update(existingKviz);
        uow.Commit();

        return existingKviz;
    }

    public async Task<Kviz> DeleteKviz(int Id)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Kviz>(uow);
        var pitanjeRepo = NewRepository<Pitanje>(uow);
        var recikliranaRepo = NewRepository<RecikliranoPitanje>(uow);

        var kvizToDelete = await kvizRepo.GetById(Id);

        foreach (var pitanje in kvizToDelete.Pitanja)
        {
            var novoReciklirano = new RecikliranoPitanje()
            {
                Sadrzaj = pitanje.Sadrzaj,
                Odgovor = pitanje.Odgovor
            };

            await recikliranaRepo.Add(novoReciklirano);
            pitanjeRepo.Delete(pitanje);
        }

        kvizRepo.Delete(kvizToDelete);
        uow.Commit();
        
        return kvizToDelete;
    }

    public async Task<ExportKvizDto> GetKvizExport(int Id)
    {
        var kviz = await NewRepository<Kviz>(NewUnitOfWork()).GetById(Id);

        return new ExportKvizDto()
        {
            Naziv = kviz.Naziv,
            Pitanja = kviz.Pitanja.Select(x => x.Sadrzaj).ToList()
        };;
    }
    
}