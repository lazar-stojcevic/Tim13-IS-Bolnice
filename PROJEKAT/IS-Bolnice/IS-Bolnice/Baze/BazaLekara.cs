using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class BazaLekara: GenerickiFajlRepozitorijum<Lekar>, LekarRepozitorijum
{
    private IRadnoVremeRepozitorijum radnoVremeRepo = new RadnoVremeFajlRepozitorijum();
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public BazaLekara() : base(@"..\..\Datoteke\lekari.txt")
    {
    }

    // metoda izlistava samo lekare opste prakse
    public List<Lekar> LekariOpstePrakse()
    {
        List<Lekar> LekariOP = new List<Lekar>();
        List<Lekar> sviLekari = DobaviSve();

        foreach(Lekar l in sviLekari)
        {
            if(l.JelLekarOpstePrakse())
            {
                LekariOP.Add(l);
            }
        }

        return LekariOP;
    }

    public List<Lekar> LekariSpecijalisti()
    {
        List<Lekar> LekariSpecijalisti = new List<Lekar>();
        List<Lekar> sviLekari = DobaviSve();

        foreach (Lekar l in sviLekari)
        {
            if (!l.JelLekarOpstePrakse())
            {
                LekariSpecijalisti.Add(l);
            }
        }

        return LekariSpecijalisti;
    }

    public List<Lekar> LekariOdredjeneOblasti(string trazenaOblast)
    {
        List<Lekar> lekariOdredjeneOblasti = new List<Lekar>();
        List<Lekar> sviLekari = DobaviSve();

        foreach (Lekar lekar in sviLekari)
        {
            if (lekar.Oblast.Naziv.Equals(trazenaOblast))
            {
                lekariOdredjeneOblasti.Add(lekar);
            }
        }
        return lekariOdredjeneOblasti;
    }

    public override Lekar KreirajEntitet(string[] delovi)
    {
        Lekar lekar = new Lekar();
        lekar.Jmbg = delovi[0];
        lekar.Id = lekar.Jmbg;
        lekar.Ime = delovi[1];
        lekar.Prezime = delovi[2];
        lekar.Oblast = new OblastLekara(delovi[3]);
        lekar.KorisnickoIme = delovi[4];
        lekar.Sifra = delovi[5];
        lekar.RadnoVreme = radnoVremeRepo.RadnoVremeOdredjenogLekara(delovi[0]);
        lekar.Ordinacija = new Soba(delovi[6]);
        return lekar;
    }

    public override string KreirajTextZaUpis(Lekar entitet)
    {
        throw new NotImplementedException();
    }


}