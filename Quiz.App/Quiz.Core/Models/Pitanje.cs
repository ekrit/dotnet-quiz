using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Core.Models;

public class Pitanje
{
    [Key]
    public int PitanjeId { get; set; }
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
    [ForeignKey(nameof(kviz))]
    public int KvizId { get; set; }
    public Kviz kviz { get; set; }
}