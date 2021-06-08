// File:    SadrzajSobeFajlRepozitorijum.cs
// Author:  teddy
// Created: Monday, April 26, 2021 3:42:44 PM
// Purpose: Definition of Class SadrzajSobeFajlRepozitorijum

using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

public class SadrzajSobeFajlRepozitorijum: GenerickiFajlRepozitorijum<SadrzajSobe>, ISadrzajSobeRepozitorijum
{

    private static string vremenskiFormatPisanje = "M/d/yyyy";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy",
        "M-d-yyyy"
    };

    public SadrzajSobeFajlRepozitorijum() : base(@"..\..\Datoteke\sadrzajiSoba.txt") { }
    

    public override SadrzajSobe KreirajEntitet(string[] podaciEntiteta)
    {
        Soba soba = new Soba();
        soba.Id = podaciEntiteta[0];
        OpremaFajlRepozitorijum opremaFajlRepozitorijum = new OpremaFajlRepozitorijum();
        Predmet predmet = opremaFajlRepozitorijum.DobaviPoId(podaciEntiteta[1]);
        Soba novaSoba = new Soba();
        SadrzajSobe s;
        if (!podaciEntiteta[3].Equals(""))
        {
            novaSoba.Id = podaciEntiteta[4];
            s = new SadrzajSobe(soba, predmet, Int32.Parse(podaciEntiteta[2]), DateTime.ParseExact(podaciEntiteta[3], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), novaSoba);
        }
        else
        {
            s = new SadrzajSobe(soba, predmet, Int32.Parse(podaciEntiteta[2]));
        }
        return s;
    }

    public override string KreirajTextZaUpis(SadrzajSobe entitet)
    {
        string tekst = entitet.Soba.Id + "#" + entitet.Predmet.Id + "#" + entitet.Kolicina + "#";
        if (entitet.DatumPremestanja.Year != 0001)
        {
            tekst = tekst + entitet.DatumPremestanja.ToString(vremenskiFormatPisanje) + "#";
        }
        if (entitet.NovaSoba != null)
        {
            tekst = tekst + entitet.NovaSoba.Id;
        }
        return tekst;
    }

    public List<SadrzajSobe> GetSadrzajSobe(string idSobe)
    {

        List<SadrzajSobe> sadrzajSobe = new List<SadrzajSobe>();
        List<SadrzajSobe> sadrzajSvihSoba = DobaviSve();
        foreach (SadrzajSobe sadrzaj in sadrzajSvihSoba)
        {
            if (sadrzaj.Soba.Id.Equals(idSobe) && sadrzaj.NovaSoba == null)
            {
                sadrzajSobe.Add(sadrzaj);
            }
        }
        return sadrzajSobe;
    }

    public List<SadrzajSobe> GetDinamickiSadrzajSobe(string idSobe)
    {
        List<SadrzajSobe> sadrzajSobe = new List<SadrzajSobe>();
        List<SadrzajSobe> sadrzajSvihSoba = DobaviSve();
        foreach (SadrzajSobe sadrzaj in sadrzajSvihSoba)
        {
            if (sadrzaj.Soba.Id.Equals(idSobe) && sadrzaj.NovaSoba == null && sadrzaj.Predmet.Tip.Equals(TipOpreme.dinamicka))
            {
                sadrzajSobe.Add(sadrzaj);
            }
        }
        return sadrzajSobe;
    }

    public string fileLocation = @"..\..\Datoteke\sadrzajiSoba.txt";

}