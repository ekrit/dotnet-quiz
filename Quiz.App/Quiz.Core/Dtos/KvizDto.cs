using System.Collections.Generic;

namespace Quiz.Core.Dtos;

public class KvizDto
{
        public int KvizId { get; set; }
        public string Naziv { get; set; }
        public List<PitanjeDto> Pitanja { get; set; }
}

public class PitanjeDto
{
    public int PitanjeId { get; set; }
    public string Sadrzaj { get; set; }
    public string Odgovor { get; set; }
}
