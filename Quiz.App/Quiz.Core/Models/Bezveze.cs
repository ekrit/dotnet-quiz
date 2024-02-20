using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Core.Models;

[Table("Bezveze")]
public class Bezveze
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}