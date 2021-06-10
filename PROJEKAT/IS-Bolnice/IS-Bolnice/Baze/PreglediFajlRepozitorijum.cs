using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class PreglediFajlRepozitorijum : GenerickiFajlRepozitorijum<Pregled>, IPregledRepozitorijum
{
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";

    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    private IzmenaTerminaFajlRepozitorijum bazaIzmena = new IzmenaTerminaFajlRepozitorijum();
    private LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();

    public PreglediFajlRepozitorijum() : base(@"..\..\Datoteke\pregledi.txt")
    {
    }

    public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
    {
        List<Pregled> sviPreglediLekara = new List<Pregled>();
        foreach (Pregled pregled in GetSve())
        {
            if (pregled.Lekar.Jmbg.Equals(jmbgLekara))
            {
                sviPreglediLekara.Add(pregled);
            }
        }

        return sviPreglediLekara;
    }

    public List<Pregled> GetSviBuduciPregledi()
    {
        List<Pregled> sviBuduciPregledi = new List<Pregled>();
        foreach (Pregled pregled in GetSve())
        {
            if (pregled.VremePocetkaPregleda > DateTime.Now.AddHours(-1))
            {
                sviBuduciPregledi.Add(pregled);
            }
        }

        return sviBuduciPregledi;
    }

    public List<Pregled> GetSviBuduciPreglediLekara(string jmbgLekara)
    {
        List<Pregled> sviPregledi = new List<Pregled>();
        foreach (Pregled pregled in GetSviBuduciPregledi())
        {
            if (pregled.Lekar.Jmbg.Equals(jmbgLekara))
            {
                sviPregledi.Add(pregled);
            }
        }

        return sviPregledi;
    }

    public List<Pregled> GetSviBuduciPreglediSobe(string idSobe)
    {
        List<Pregled> preglediUSobi = new List<Pregled>();
        foreach (Pregled pregled in GetSviBuduciPregledi())
        {
            if (pregled.Lekar.Ordinacija.Id.Equals(idSobe))
            {
                preglediUSobi.Add(pregled);
            }
        }
        return preglediUSobi;
    }

    public List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta)
    {
        List<Pregled> pregledi = new List<Pregled>();

        foreach (Pregled pregled in GetSviBuduciPregledi())
        {
            if (pregled.Pacijent.Jmbg.Equals(jmbgPacijenta))
            {
                pregledi.Add(pregled);
            }
        }

        return pregledi;
    }

    public override Pregled KreirajEntitet(string[] podaciEntiteta)
    {
        LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();
        PacijentFajlRepozitorijum pacijentFajlRepozitorijum = new PacijentFajlRepozitorijum();
        Lekar lekar = lekarFajlRepozitorijum.GetPoId(podaciEntiteta[2]);
        Pregled p = new Pregled();
        p.Id = podaciEntiteta[0];
        Pacijent pac = pacijentFajlRepozitorijum.GetPoJmbg(podaciEntiteta[1]);
        p.Pacijent = pac;
        p.VremePocetkaPregleda = DateTime.ParseExact(podaciEntiteta[3], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        p.VremeKrajaPregleda = DateTime.ParseExact(podaciEntiteta[4], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        p.Lekar = lekar;

        return p;
    }

    public override string KreirajTextZaUpis(Pregled pregled)
    {
        
        foreach (Lekar lekar in lekarFajlRepozitorijum.GetSve())
        {
            if (lekar.Jmbg.Equals(pregled.Lekar.Jmbg))
            {
                pregled.Lekar.Ordinacija = lekar.Ordinacija;
                break;
            }
        }
        

        return pregled.Id + "#" + pregled.Pacijent.Jmbg + "#" + pregled.Lekar.Jmbg + "#" + pregled.VremePocetkaPregleda.ToString(vremenskiFormatPisanje)
               + "#" + pregled.VremeKrajaPregleda.ToString(vremenskiFormatPisanje) + "#" + pregled.Lekar.Ordinacija.Id;
    }
}