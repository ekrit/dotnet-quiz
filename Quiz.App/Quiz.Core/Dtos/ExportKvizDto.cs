using System.Collections.Generic;

namespace Quiz.Core.Dtos;

public class ExportKvizDto
{
    public string Naziv { get; set; }
    public List<string> Pitanja { get; set; }
}