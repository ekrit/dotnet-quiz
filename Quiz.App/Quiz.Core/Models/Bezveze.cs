using System.ComponentModel.DataAnnotations;

namespace Quiz.Core.Models;

public class Bezveze
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}