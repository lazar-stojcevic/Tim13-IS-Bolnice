using System.Collections.Generic;

public class Pacijent : Korisnik
{
    public Lekar IzabraniLekar { get; set; }
    public bool Guest { get; set; }
    public List<Sastojak> Alergeni { get; set; }

    public Pacijent() 
    {
        Alergeni = new List<Sastojak>();    
    }

    public Pacijent(string jmbg)
    {
        Jmbg = jmbg;
    }

    public Pacijent(string jmbg, string ime, string prezime)
    {
        Jmbg = jmbg;
        Ime = ime;
        Prezime = prezime;
        Pol = Pol.neodredjen;
        Alergeni = new List<Sastojak>();
    }
}