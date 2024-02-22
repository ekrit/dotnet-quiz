using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Core.Models;

public class Kviz
{
    [Key]
    public int KvizId { get; set; }
    public string Naziv { get; set; }
    public List<Pitanje> Pitanja { get; set; }
}