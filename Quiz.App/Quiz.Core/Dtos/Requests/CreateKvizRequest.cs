using System.Collections.Generic;

namespace Quiz.Core.Dtos.Requests;

public class CreateKvizRequest
{
    public string Naziv { get; set; }
    public List<CreatePitanje> Pitanja { get; set; }
}

public class CreatePitanje
{
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
}