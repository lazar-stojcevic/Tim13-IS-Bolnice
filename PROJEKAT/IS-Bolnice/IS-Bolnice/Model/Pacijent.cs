// File:    Pacijent.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pacijent

using System;

public class Pacijent : Korisnik
{
    private Lekar izabraniLekar;
    private bool guest = false;

    public Pacijent(Lekar izbraniLekar, bool guest) : base()
    {
        this.izabraniLekar = izbraniLekar;
        this.guest = guest;
    }

    public Pacijent(string jmbg, string korisnickoIme, string sifra, string ime, string prezime, string brojTelefona,
       string eMail, string adresa, Pol pol, Lekar izabraniLekar) : base(jmbg, korisnickoIme, sifra, ime, prezime,
           brojTelefona, eMail, adresa, pol)
    {
        this.izabraniLekar = izabraniLekar;
    }

    public Pacijent(Korisnik korisnik, Lekar izabraniLekar) : base(korisnik)
    {
        this.izabraniLekar = izabraniLekar;
    }
    public Pacijent()
        { }
    public Lekar IzabraniLekar { get; set; }
    public bool Guest { get; set; }
}