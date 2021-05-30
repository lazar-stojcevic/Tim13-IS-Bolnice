using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

public class BazaPregleda
{
    private static string fileLocation = @"..\..\Datoteke\pregledi.txt";
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    private BazaIzmena bazaIzmena = new BazaIzmena();
    private LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();

    //PREDLAZE TERMINE ZA IZMENU, SREDITI PROSLEDJENOG LEKARA DA IMA TACNO RADNO VREME
    public List<Pregled> SlobodniTerminiZaIzmenu(Lekar l, DateTime datum)
    {
        List<Pregled> validni = new List<Pregled>();
        List<Pregled> slobodni = new List<Pregled>();

        List<Lekar> lekari = lekarFajlRepozitorijum.LekariOpstePrakse();
        Lekar lekar = new Lekar();
        foreach (Lekar ll in lekari)
        {
            if (l.Jmbg == ll.Jmbg)
            {
                lekar = ll;
            }
        }

        System.DateTime pocetakIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
        System.DateTime krajIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);
        krajIntervala = krajIntervala.AddMinutes(-30);

        while (pocetakIntervala <= krajIntervala)
        {
            Pregled p = new Pregled();
            p.Lekar = lekar;
            p.VremePocetkaPregleda = pocetakIntervala;
            pocetakIntervala = pocetakIntervala.AddMinutes(10);
            p.VremeKrajaPregleda = p.VremePocetkaPregleda.AddMinutes(30);
            slobodni.Add(p);
        }

        foreach (Pregled predlozeni in slobodni)
        {
            bool isValid = true;
            foreach (Pregled zakazani in SviBuduciPreglediKojeLekarIma(l.Jmbg))
            {
                // TODO : refaktorisati (moze se pozvati metoda PreklapanjeTerminaPregleda(Pregled predlozeniPregled, Pregled zakazaniPregled) )
                if (predlozeni.VremePocetkaPregleda == zakazani.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                validni.Add(predlozeni);
            }
        }

        return validni;
    }

    public List<Pregled> SviBuduciPreglediKojeLekarIma(string jmbgLekara)
    {
        List<Pregled> pregledi = new List<Pregled>();

        foreach (Pregled p in SviBuduciPregledi())
        {
            if (p.Lekar.Jmbg.Equals(jmbgLekara) && p.VremeKrajaPregleda > DateTime.Now)
            {
                pregledi.Add(p);
            }
        }

        return pregledi;
    }

    public List<Pregled> SviPregledi()
    {
        LekarFajlRepozitorijum bl = new LekarFajlRepozitorijum();

        PacijentFajlRepozitorijum pacijentFajlRepozitorijum = new PacijentFajlRepozitorijum();
        List<Pacijent> pacijenti = pacijentFajlRepozitorijum.DobaviSve();

        List<string> lines = new List<string>();
        List<Pregled> pregledi = new List<Pregled>();
        List<Lekar> lekari = bl.DobaviSve();

        lines = File.ReadAllLines(fileLocation).ToList();

        foreach (string line in lines)
        {
            string[] items = line.Split('#');

            Pregled p = new Pregled();
            Pacijent pac = new Pacijent();

            string jmbgPacijenta = items[0];
            string jmbgLekara = items[1];
            string vremePocetka = items[2];
            string vremeKraja = items[3];
            pac.Jmbg = jmbgPacijenta;
            p.Pacijent = pac;
            p.VremePocetkaPregleda = DateTime.ParseExact(vremePocetka, vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            p.VremeKrajaPregleda = DateTime.ParseExact(vremeKraja, vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            //dodati broj sobe ovde
            foreach (Lekar l in lekari)
            {
                if (l.Jmbg == jmbgLekara)
                {
                    p.Lekar = l;
                }
            }

            //DODATO NA BOJANOV KOD
            foreach (Pacijent pacijent in pacijenti)
            {
                if (p.Pacijent.Jmbg.Equals(pacijent.Jmbg))
                {
                    p.Pacijent = pacijent;
                    break;
                }
            }

            pregledi.Add(p);
            }

        return pregledi;
    }

    //OPTIMIZOVATI KOD
    public List<Pregled> SviBuduciPregledi()
    {
        List<Pregled> sviBuduciPregledi = new List<Pregled>();
        foreach (Pregled pregled in SviPregledi())
        {
            if (pregled.VremePocetkaPregleda > DateTime.Now.AddHours(-1))
            {
                sviBuduciPregledi.Add(pregled);
            }
        }

        return sviBuduciPregledi;
    }

    public void ZakaziPregled(Pregled noviPregled)
    {
        string zakazivanje = FormatPisanjaPregleda(noviPregled);

        List<string> pregledi = new List<string>();
        pregledi.Add(zakazivanje);
        File.AppendAllLines(fileLocation, pregledi);

        SaveChangeInBase(noviPregled.Pacijent.Jmbg);
    }

    // refaktorisane
    public void OtkaziPregled(Pregled pregledZaBrisanje)
    {
        List<string> redoviZaUpisUDatoteku = new List<string>();

        foreach (Pregled pregled in SviBuduciPregledi())
        {
            if (pregled.Lekar.Jmbg != pregledZaBrisanje.Lekar.Jmbg || !pregled.VremePocetkaPregleda.Equals(pregledZaBrisanje.VremePocetkaPregleda))
            {
                string zakazivanje = FormatPisanjaPregleda(pregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
            }

        }
        File.WriteAllLines(fileLocation, redoviZaUpisUDatoteku);
    }

    public void IzmeniPregled(Pregled noviPregled, Pregled stariPregled)
    {
        List<string> redoviZaUpisUDatoteku = new List<string>();

        foreach (Pregled pregled in SviBuduciPregledi())
        {
            if (pregled.Lekar.Jmbg != stariPregled.Lekar.Jmbg || !pregled.VremePocetkaPregleda.Equals(stariPregled.VremePocetkaPregleda))
            {
                string zakazivanje = FormatPisanjaPregleda(pregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
            }
            else
            {
                string zakazivanje = FormatPisanjaPregleda(noviPregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
                SaveChangeInBase(noviPregled.Pacijent.Jmbg);
            }

        }
        OtkaziPregled(stariPregled);
       ZakaziPregled(noviPregled);
    }

    public string FormatPisanjaPregleda(Pregled pregled){
        foreach (Lekar lekar in lekarFajlRepozitorijum.DobaviSve())
        {
            if (lekar.Jmbg.Equals(pregled.Lekar.Jmbg))
            {
                pregled.Lekar.Ordinacija = lekar.Ordinacija;
                break;
            }
        }
    
        return pregled.Pacijent.Jmbg + "#" + pregled.Lekar.Jmbg + "#" + pregled.VremePocetkaPregleda.ToString(vremenskiFormatPisanje)
            + "#" + pregled.VremeKrajaPregleda.ToString(vremenskiFormatPisanje) + "#" + pregled.Lekar.Ordinacija.Id;
    }

    public void SaveChangeInBase(string jmbgPacijenta)
    {
        Change change = new Change();
        DateTime now = DateTime.Now;

        change.DateOfChange = now;
        change.JmbgOfPatient = jmbgPacijenta;

        bazaIzmena.SaveChange(change);
    }
}