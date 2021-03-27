// File:    Korisnik.cs
// Author:  Zola
// Created: Monday, March 22, 2021 7:26:17 PM
// Purpose: Definition of Class Korisnik

using System;

public class Korisnik
{
    private string jmbg;
    private string korisnickoIme;
    private string sifra;
    private string ime;
    private string prezime;
    private string brojTelefona;
    private string eMail;
    private string adresa;
    private Pol pol;
    private bool obrisan = false;

    public Korisnik(string jmbg, string korisnickoIme, string sifra, string ime, string prezime, string brojTelefona,
        string eMail, string adresa, Pol pol)
    {
        this.jmbg = jmbg;
        this.korisnickoIme = korisnickoIme;
        this.sifra = sifra;
        this.ime = ime;
        this.prezime = prezime;
        this.brojTelefona = brojTelefona;
        this.eMail = eMail;
        this.adresa = adresa;
        this.pol = pol;
    }

    public Korisnik(Korisnik korisnik)
    {
        this.jmbg = korisnik.jmbg;
        this.korisnickoIme = korisnik.korisnickoIme;
        this.sifra = korisnik.sifra;
        this.ime = korisnik.ime;
        this.prezime = korisnik.prezime;
        this.brojTelefona = korisnik.brojTelefona;
        this.eMail = korisnik.eMail;
        this.adresa = korisnik.adresa;
        this.pol = korisnik.pol;
    }

    public Korisnik()
    {

    }

    public string Jmbg { get; set; }
    public string KorisnickoIme { get; set; }
    public string Sifra { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string BrojTelefona { get; set; }
    public string EMail { get; set; }
    public string Adresa { get; set; }
    public Pol Pol { get; set; }
    public bool Obrisan { get; set; }
}