using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class BazaOperacija:GenerickiFajlRepozitorijum<Operacija>, OperacijaRepozitorijum
{
    public string fileLocation;
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public BazaOperacija() : base(@"..\..\Datoteke\operacije.txt")
    {
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
        foreach (Lekar lekar in new BazaLekara().DobaviSve())
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
        foreach (Soba s in new BazaBolnica().GetSobe())
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
        foreach (Pacijent p in new PacijentFajlRepozitorijum().DobaviSve())
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

    public List<Operacija> SveSledeceOperacije()
    {
        List<Operacija> ret = new List<Operacija>();
        foreach (Operacija operacija in DobaviSve())
        {
            if (operacija.VremePocetkaOperacije > DateTime.Now.AddHours(-1))
            {
                ret.Add(operacija);
            }
        }

        return ret;
    }
}