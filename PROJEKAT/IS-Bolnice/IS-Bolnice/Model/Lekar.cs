using IS_Bolnice.Model;
using System;

public class Lekar : Korisnik
{
    public OblastLekara Oblast { get; set; }
    public Soba Ordinacija { get; set; }
    public RadnoVremeLekara RadnoVreme { get; set; }


    public Lekar()
    {
        Id = Jmbg;
    }

    public Lekar(string jmbg)
    {
        Jmbg = jmbg;
        Id = jmbg;
    }
}