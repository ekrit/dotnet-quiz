using System.Collections.Generic;
using Quiz.Core.Models;

namespace Quiz.Core.Dtos.Requests;

public class EditKvizRequest
{
    public int KvizId { get; set; }
    public string Naziv { get; set; }
    public List<EditPitanjeRequest> Pitanja { get; set; }
}

public class EditPitanjeRequest
{
    public int PitanjeId { get; set; }
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
}