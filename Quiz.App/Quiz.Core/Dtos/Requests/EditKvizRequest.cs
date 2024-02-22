using System.Collections.Generic;
using Quiz.Core.Models;

namespace Quiz.Core.Dtos.Requests;

public class EditKvizRequest
{
    public int KvizId { get; set; }
    public string Naziv { get; set; }
    public List<Pitanje> Pitanja { get; set; }
}