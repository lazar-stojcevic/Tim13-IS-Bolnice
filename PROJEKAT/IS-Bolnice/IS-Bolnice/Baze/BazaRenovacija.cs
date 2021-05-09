// File:    BazaRenovacija.cs
// Author:  teddy
// Created: Friday, May 7, 2021 6:46:55 PM
// Purpose: Definition of Class BazaRenovacija

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

public class BazaRenovacija
{
   private string fileLocation = @"..\..\Datoteke\renoviranje.txt";

    private static string vremenskiFormatPisanje = "M/d/yyyy";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy",
        "M-d-yyyy"
    };

    public List<Renovacija> SveRenovacije()
   {
        List<Renovacija> ret = new List<Renovacija>();
        if (File.Exists(fileLocation))
        {
            string[] lines = File.ReadAllLines(fileLocation);
            foreach (string line in lines)
            {
                Renovacija s = ParseFromString(line);
                ret.Add(s);
            }
        }
        else
        {

            MessageBox.Show("Nista");
        }
        return ret;
    }

    public List<Renovacija> SveRenovacijeJedneSobe(Soba soba)
    {
        List<Renovacija> sveRenovacije = SveRenovacije();
        List<Renovacija> renovacijeJedneSobe = new List<Renovacija>();

        foreach (Renovacija renovacija in sveRenovacije)
        {
            if (renovacija.ProstorijaZaRenoviranje.Jednaka(soba))
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

    public void KreirajRenovaciju(Renovacija renovacija)
   {
        if (File.Exists(fileLocation))
        {
            string novoRenoviranje = ParseToString(renovacija);
            List<string> lista = new List<string>();
            lista.Add(novoRenoviranje);
            File.AppendAllLines(fileLocation, lista);
        }
        else
        {

            MessageBox.Show("Nista");
        }
    }

    public string ParseToString(Renovacija renovacija)
    {
        string tekst = renovacija.ProstorijaZaRenoviranje.Id + "#" + renovacija.DatumPocetka.ToString(vremenskiFormatPisanje) + "#" + renovacija.DatumKraja.ToString(vremenskiFormatPisanje);
        return tekst;
    }

    public void IzmeniRenovaciju(Renovacija renovacija)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiRenovaciju(Renovacija renovacija)
   {
        if (File.Exists(fileLocation))
        {
            List<Renovacija> listaRenovacija =SveRenovacije();
            List<string> tekst = new List<string>();
            foreach (Renovacija renovacijaPrivremena in listaRenovacija)
            {
                if (!(renovacijaPrivremena.ProstorijaZaRenoviranje.Id.Equals(renovacija.ProstorijaZaRenoviranje.Id)))
                {
                    string linija = ParseToString(renovacijaPrivremena);
                    tekst.Add(linija);
                }
            }
            File.WriteAllLines(fileLocation, tekst);
        }
        else
        {

            MessageBox.Show("Nista");
        }
    }

}