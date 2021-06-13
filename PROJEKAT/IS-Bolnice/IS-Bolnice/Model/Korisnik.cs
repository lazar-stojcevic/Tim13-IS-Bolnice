using System;
using IS_Bolnice.Model;

public class Korisnik: Entitet
{
    public string Jmbg { get; set; }
    public string KorisnickoIme { get; set; }
    public string Sifra { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string BrojTelefona { get; set; }
    public string EMail { get; set; }
    public string Adresa { get; set; }
    public Pol Pol { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public bool Obrisan { get; set; }
}