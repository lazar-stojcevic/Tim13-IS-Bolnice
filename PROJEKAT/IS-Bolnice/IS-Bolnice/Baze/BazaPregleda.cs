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
    }

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
}