using System;

public class Sastojak
{
    public string Ime { get; set; }

    public Sastojak(string ime)
    {
        Ime = ime;
    }

    public bool Isti(Sastojak sastojak)
    {
        return Ime.Equals(sastojak.Ime);
    }
}