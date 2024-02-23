using System.Collections.Generic;

namespace Quiz.Core.Dtos.Requests;

public class CreateKvizRequest
{
    public string Naziv { get; set; }
    public List<CreatePitanjeRequest> Pitanja { get; set; }
}

public class CreatePitanjeRequest
{
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
}