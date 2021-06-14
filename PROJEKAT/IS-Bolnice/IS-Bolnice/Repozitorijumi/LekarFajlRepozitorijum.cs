using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;

public class LekarFajlRepozitorijum: GenerickiFajlRepozitorijum<Lekar>, ILekarRepozitorijum
{
    private IRadnoVremeRepozitorijum radnoVremeRepo = new RadnoVremeFajlRepozitorijum();

    public LekarFajlRepozitorijum() : base(@"..\..\Datoteke\lekari.txt")
    {
    }

    public List<Lekar> GetSviLekariOpstePrakse()
    {
        List<Lekar> listaSvihLekaraOpstePrakse = new List<Lekar>();
        foreach (Lekar lekar in GetSve())
        {
            if (lekar.Oblast.JelOpstaPraksa())
            {
                listaSvihLekaraOpstePrakse.Add(lekar);
            }
        }

        return listaSvihLekaraOpstePrakse;
    }

    public List<Lekar> GetSviLekariSpecijalisti()
    {
        List<Lekar> listaSvihLekara = new List<Lekar>();
        foreach (Lekar lekar in GetSve())
        {
            if (!lekar.Oblast.JelOpstaPraksa())
            {
                listaSvihLekara.Add(lekar);
            }
        }

        return listaSvihLekara;
    }

    public List<Lekar> LekariOdredjeneOblasti(string trazenaOblast)
    {
        List<Lekar> lekariOdredjeneOblasti = new List<Lekar>();
        List<Lekar> sviLekari = GetSve();

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
        lekar.RadnoVreme = radnoVremeRepo.GetRadnoVremeOdredjenogLekara(delovi[0]);
        lekar.Ordinacija = new Soba(delovi[6]);
        return lekar;
    }

    public override string KreirajTextZaUpis(Lekar entitet)
    {
        throw new NotImplementedException();
    }


}