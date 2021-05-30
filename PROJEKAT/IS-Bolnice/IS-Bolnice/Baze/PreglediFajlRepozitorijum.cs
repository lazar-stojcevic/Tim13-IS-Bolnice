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

    private BazaIzmena bazaIzmena = new BazaIzmena();
    private LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();

    public PreglediFajlRepozitorijum() : base(@"..\..\Datoteke\pregledi.txt")
    {
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

    public List<Pregled> SviBuduciPregledi()
    {
        List<Pregled> sviBuduciPregledi = new List<Pregled>();
        foreach (Pregled pregled in DobaviSve())
        {
            if (pregled.VremePocetkaPregleda > DateTime.Now.AddHours(-1))
            {
                sviBuduciPregledi.Add(pregled);
            }
        }

        return sviBuduciPregledi;
    }

    

    public override Pregled KreirajEntitet(string[] podaciEntiteta)
    {
        LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();
        PacijentFajlRepozitorijum pacijentFajlRepozitorijum = new PacijentFajlRepozitorijum();
        Lekar lekar = lekarFajlRepozitorijum.DobaviPoId(podaciEntiteta[2]);
        Pregled p = new Pregled();
        p.Id = podaciEntiteta[0];
        Pacijent pac = pacijentFajlRepozitorijum.DobaviPoJmbg(podaciEntiteta[1]);
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
        
        foreach (Lekar lekar in lekarFajlRepozitorijum.DobaviSve())
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