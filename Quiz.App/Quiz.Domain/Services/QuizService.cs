using Quiz.Core;
using Quiz.Core.Dtos;
using Quiz.Core.Dtos.Requests;
using Quiz.Core.Interfaces;
using Quiz.Core.Models;
using Quiz.Infrastructure.Common;

namespace Quiz.Domain.Services;

public class QuizService : DataService, IQuizService
{
    private readonly IKvizRepositoryFactory _kvizRepositoryFactory;

    public QuizService(IUnitOfWorkFactory iUnitOfWorkFactory, 
        IRepositoryFactory iRepositoryFactory,
        IKvizRepositoryFactory kvizRepositoryFactory) : base(iUnitOfWorkFactory, iRepositoryFactory)
    {
        _kvizRepositoryFactory = kvizRepositoryFactory;
    }

    public IEnumerable<KvizDto> GetAllKvizovi()
    {
        var kvizovi = NewRepository<Kviz>(NewUnitOfWork()).GetAll(x=>x.Pitanja);

        return kvizovi.Select(kviz => new KvizDto()
            {
                KvizId = kviz.KvizId,
                Naziv = kviz.Naziv,
                Pitanja = kviz.Pitanja.Select(x => new PitanjeDto()
                    {
                        PitanjeId = x.PitanjeId,
                        Sadrzaj = x.Sadrzaj,
                        Odgovor = x.Odgovor
                    }
                ).ToList()
            }
        ).ToList();
    }

    public KvizDto GetKvizById(int id)
    {
        var kviz = _kvizRepositoryFactory.Create(NewUnitOfWork()).GetKvizById(id);
        
        return new KvizDto()
        {
            KvizId = kviz.KvizId,
            Naziv = kviz.Naziv,
            Pitanja = kviz.Pitanja.Select(x => new PitanjeDto()
                {
                    PitanjeId = x.PitanjeId,
                    Sadrzaj = x.Sadrzaj,
                    Odgovor = x.Odgovor
                }
            ).ToList()
        };
    }

    public IEnumerable<PitanjeDto> GetAllPitanja()
    {
        var pitanja = NewRepository<Pitanje>(NewUnitOfWork()).GetAll();

        return pitanja.Select(data => new PitanjeDto()
            {
                PitanjeId = data.PitanjeId,
                Sadrzaj = data.Sadrzaj,
                Odgovor = data.Odgovor
            }
        ).ToList();
    }

    public IEnumerable<RecikliranoPitanje> GetAllReciklirana()
    {
        return NewRepository<RecikliranoPitanje>(NewUnitOfWork()).GetAll();
    }

    public async Task<KvizDto> CreateKviz(CreateKvizRequest noviKvizRequest)
    {
        var uow = NewUnitOfWork(Enums.UnitOfWorkMode.Writable);
        var kvizRepo = NewRepository<Kviz>(uow);
        var pitanjeRepo = NewRepository<Pitanje>(uow);
        var recikliranaRepo = NewRepository<RecikliranoPitanje>(uow);

        var recikliranaPitanja = recikliranaRepo.GetAll();

        var noviKviz = new Kviz()
        {
            Naziv = noviKvizRequest.Naziv
        };

        await kvizRepo.Add(noviKviz);
        await uow.Commit();

        var novaPitanja = new List<Pitanje>();

        foreach (var pitanje in noviKvizRequest.Pitanja)
        {
            var existingQuestion = recikliranaPitanja.FirstOrDefault(x => x.Sadrzaj == pitanje.Sadrzaj);
            if (existingQuestion != null)
            {
                var novoPitanje = new Pitanje()
                {
                    Sadrzaj = pitanje.Sadrzaj,
                    Odgovor = pitanje.Odgovor,
                    KvizId = noviKviz.KvizId
                };

                await pitanjeRepo.Add(novoPitanje);
                recikliranaRepo.Delete(existingQuestion);

                novaPitanja.Add(novoPitanje);
            }
            else
            {
                var novoPitanje = new Pitanje()
                {
                    Sadrzaj = pitanje.Sadrzaj,
                    Odgovor = pitanje.Odgovor,
                    KvizId = noviKviz.KvizId
                };

                await pitanjeRepo.Add(novoPitanje);

                novaPitanja.Add(novoPitanje);
            }
        }

        await uow.Commit();
        noviKviz.Pitanja = novaPitanja;

        kvizRepo.Update(noviKviz);
        await uow.Commit();

        return new KvizDto()
        {
            KvizId = noviKviz.KvizId,
            Naziv = noviKviz.Naziv,
            Pitanja = noviKviz.Pitanja.Select(data => new PitanjeDto()
                {
                    PitanjeId = data.PitanjeId,
                    Sadrzaj = data.Sadrzaj,
                    Odgovor = data.Odgovor
                }
            ).ToList()
        };
    }

    public async Task<KvizDto> EditKviz(EditKvizRequest editKvizRequest)
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
                dbQuestion = new Pitanje
                {
                    Sadrzaj = editPitanje.Sadrzaj, 
                    Odgovor = editPitanje.Odgovor,
                    KvizId = existingKviz.KvizId
                };
                await pitanjeRepo.Add(dbQuestion);
            }

            existingKviz.Pitanja.Add(dbQuestion);
        }
        
        await uow.Commit();

        kvizRepo.Update(existingKviz);
        await uow.Commit();

        return new KvizDto()
        {
            KvizId = existingKviz.KvizId,
            Naziv = existingKviz.Naziv,
            Pitanja = existingKviz.Pitanja.Select(data => new PitanjeDto()
                {
                    PitanjeId = data.PitanjeId,
                    Sadrzaj = data.Sadrzaj,
                    Odgovor = data.Odgovor
                }
            ).ToList()
        };
    }

    public async Task<KvizDto> DeleteKviz(int Id)
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
        await uow.Commit();

        kvizRepo.Delete(kvizToDelete);
        await uow.Commit();
        
        return new KvizDto()
        {
            KvizId = kvizToDelete.KvizId,
            Naziv = kvizToDelete.Naziv,
            Pitanja = kvizToDelete.Pitanja.Select(data => new PitanjeDto()
                {
                    PitanjeId = data.PitanjeId,
                    Sadrzaj = data.Sadrzaj,
                    Odgovor = data.Odgovor
                }
            ).ToList()
        };
    }

    public async Task<ExportKvizDto> GetKvizExport(int Id)
    {
        var kviz = await NewRepository<Kviz>(NewUnitOfWork()).GetById(Id);

        return new ExportKvizDto()
        {
            Naziv = kviz.Naziv,
            Pitanja = kviz.Pitanja.Select(x => x.Sadrzaj).ToList()
        };
    }
    
}