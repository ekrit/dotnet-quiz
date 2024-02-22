using System.ComponentModel.DataAnnotations;

namespace Quiz.Core.Models;

public class RecikliranoPitanje
{
    [Key]
    public int RecikliranoPitanjeId { get; set; }
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
}