using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class OperacijaFajlRepozitorijum:GenerickiFajlRepozitorijum<Operacija>, IOperacijaRepozitorijum
{
    public string fileLocation;
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public OperacijaFajlRepozitorijum() : base(@"..\..\Datoteke\operacije.txt")
    {
    }

    public List<Operacija> GetSveOperacijeLekara(string jmbgLekara)
    {
        List<Operacija> ret = new List<Operacija>();
        foreach (Operacija operacija in GetSve())
        {
            if (operacija.Lekar.Jmbg.Equals(jmbgLekara))
            {
                ret.Add(operacija);
            }
        }
        return ret;
    }

    public List<Operacija> GetSveBuduceOperacije()
    {
        List<Operacija> ret = new List<Operacija>();
        foreach (Operacija operacija in GetSve())
        {
            if (operacija.VremePocetkaOperacije > DateTime.Now.AddHours(-1))
            {
                ret.Add(operacija);
            }
        }

        return ret;
    }

    public List<Operacija> GetSveBuduceOperacijePacijenta(string jmbgPacijenta)
    {
        List<Operacija> operacijePacijenta = new List<Operacija>();

        foreach (Operacija o in GetSveBuduceOperacije())
        {
            if (o.Pacijent.Jmbg.Equals(jmbgPacijenta))
            {
                operacijePacijenta.Add(o);
            }
        }

        return operacijePacijenta;
    }

    public List<Operacija> GetSveBuduceOperacijeSale(string idSale)
    {
        List<Operacija> operacijeSale = new List<Operacija>();
        foreach (Operacija operacija in GetSveBuduceOperacije())
        {
            if (operacija.Soba.Id.Equals(idSale))
            {
                operacijeSale.Add(operacija);
            }
        }

        return operacijeSale;
    }

    public List<Operacija> GetSveBuduceOperacijeLekara(string jmbgLekara)
    {
        List<Operacija> ret = new List<Operacija>();
        foreach (Operacija operacija in GetSveBuduceOperacije())
        {
            if (operacija.Lekar.Jmbg.Equals(jmbgLekara))
            {
                ret.Add(operacija);
            }
        }
        return ret;
    }

    public override Operacija KreirajEntitet(string[] delovi)
    {
        Operacija operacija = new Operacija(delovi[0]);

        PostaviIntervalTrajanjaOperacije(delovi, operacija);

        PostaviPacijentaOperacije(delovi, operacija);

        PostaviOperacionuSalu(delovi, operacija);
        
        PostaviLekaraOperacije(delovi, operacija);

        //?
        operacija.Soba.Id = delovi[5];

        PostaviHitnostOperacije(delovi, operacija);

        return operacija;
    }

    private static void PostaviHitnostOperacije(string[] delovi, Operacija operacija)
    {
        if (delovi[6].Equals("True"))
        {
            operacija.Hitna = true;
        }
        else operacija.Hitna = false;
    }

    private static void PostaviLekaraOperacije(string[] delovi, Operacija operacija)
    {
        operacija.Lekar.Jmbg = delovi[4];
        foreach (Lekar lekar in new LekarFajlRepozitorijum().GetSve())
        {
            if (delovi[4].Equals(lekar.Jmbg))
            {
                operacija.Lekar = lekar;
                break;
            }
        }
    }

    private static void PostaviOperacionuSalu(string[] delovi, Operacija operacija)
    {
        foreach (Soba s in new BolnicaFajlRepozitorijum().GetSobe())
        {
            if (delovi[5].Equals(s.Id))
            {
                operacija.Soba = s;
                break;
            }
        }
    }

    private static void PostaviPacijentaOperacije(string[] delovi, Operacija operacija)
    {
        operacija.Pacijent.Jmbg = delovi[3];
        foreach (Pacijent p in new PacijentFajlRepozitorijum().GetSve())
        {
            if (operacija.Pacijent.Jmbg.Equals(p.Jmbg))
            {
                operacija.Pacijent = p;
                break;
            }
        }
    }

    private static void PostaviIntervalTrajanjaOperacije(string[] delovi, Operacija o)
    {
        o.VremePocetkaOperacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        o.VremeKrajaOperacije = DateTime.ParseExact(delovi[2], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }

    public override string KreirajTextZaUpis(Operacija novaOperacija)
    {
        string linija = novaOperacija.Id + "#" + novaOperacija.VremePocetkaOperacije.ToString(vremenskiFormatPisanje) +
                        "#" + novaOperacija.VremeKrajaOperacije.ToString(vremenskiFormatPisanje) + "#" +
                        novaOperacija.Pacijent.Jmbg + "#" + novaOperacija.Lekar.Jmbg + "#" + novaOperacija.Soba.Id +
                        "#" + novaOperacija.Hitna;
        return linija;
    }
}