using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

public class RenovacijaFajlRepozitorijum: GenerickiFajlRepozitorijum<Renovacija>, IRenovacijaRepozitorijum
{

    private static string vremenskiFormatPisanje = "M/d/yyyy";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy",
        "M-d-yyyy"
    };

    public RenovacijaFajlRepozitorijum() : base(@"..\..\Datoteke\renoviranje.txt") { }
 
    public List<Renovacija> SveRenovacijeJedneSobe(string id)
    {
        List<Renovacija> sveRenovacije = GetSve();
        List<Renovacija> renovacijeJedneSobe = new List<Renovacija>();

        foreach (Renovacija renovacija in sveRenovacije)
        {
            if (renovacija.ProstorijaZaRenoviranje.Id.Equals(id))
            {
                renovacijeJedneSobe.Add(renovacija);
            }
        }

        return renovacijeJedneSobe;
    }

    public Renovacija ParseFromString(string line)
    {

        string[] niz = line.Split('#');
        Soba soba = new Soba();
        soba.Id = niz[0];
        Renovacija novaRenovacija = new Renovacija(DateTime.ParseExact(niz[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), DateTime.ParseExact(niz[2], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), soba);
        return novaRenovacija;
    }


    public override Renovacija KreirajEntitet(string[] podaciEntiteta)
    {
        Soba soba = new Soba();
        soba.Id = podaciEntiteta[0];
        Renovacija novaRenovacija = new Renovacija(DateTime.ParseExact(podaciEntiteta[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), DateTime.ParseExact(podaciEntiteta[2], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), soba);
        return novaRenovacija;
    }

    public override string KreirajTextZaUpis(Renovacija entitet)
    {
        string tekst = entitet.ProstorijaZaRenoviranje.Id + "#" + entitet.DatumPocetka.ToString(vremenskiFormatPisanje) + "#" + entitet.DatumKraja.ToString(vremenskiFormatPisanje);
        return tekst;
    }
}