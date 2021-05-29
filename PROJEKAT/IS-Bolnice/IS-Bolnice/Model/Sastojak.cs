using System;
using IS_Bolnice.Model;

public class Sastojak: Entitet
{
    public string Ime { get; set; }

    public Sastojak(string ime):base(ime)
    {
        Ime = ime;
    }

    public bool Isti(Sastojak sastojak)
    {
        return Ime.Equals(sastojak.Ime);
    }
}