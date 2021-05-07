// File:    BazaSadrzaja.cs
// Author:  teddy
// Created: Monday, April 26, 2021 3:42:44 PM
// Purpose: Definition of Class BazaSadrzaja

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class BazaSadrzaja
{
   public List<SadrzajSobe> SviSadrzaji()
   {
        List<SadrzajSobe> ret = new List<SadrzajSobe>();
        if (File.Exists(fileLocation))
        {
            string[] lines = File.ReadAllLines(fileLocation);
            foreach (string line in lines)
            {
                SadrzajSobe s = ParseFromString(line);
                ret.Add(s);
            }
        }
        else
        {

            MessageBox.Show("Nista");
        }
        return ret;
    }

    public SadrzajSobe ParseFromString(string line) {

        string[] niz = line.Split('#');
        Soba soba = new Soba();
        soba.Id = niz[0];
        Predmet predmet = new Predmet();
        predmet.Id = niz[1];
        Soba novaSoba = new Soba();
        SadrzajSobe s;
        if (!niz[3].Equals(""))
        {
            novaSoba.Id = niz[4];
            s = new SadrzajSobe(soba, predmet, Int32.Parse(niz[2]), DateTime.Parse(niz[3]), novaSoba);
        }
        else {
            s = new SadrzajSobe(soba.Id, predmet.Id, Int32.Parse(niz[2]));
        }
        return s;
    }

    public List<SadrzajSobe> GetSadrzajSobe(string idSobe) {

        List<SadrzajSobe> sadrzajSobe = new List<SadrzajSobe>();
        List<SadrzajSobe> sadrzajSvihSoba = SviSadrzaji() ;
        foreach (SadrzajSobe sadrzaj in sadrzajSvihSoba) {
            if (sadrzaj.Soba.Id.Equals(idSobe)) {
                sadrzajSobe.Add(sadrzaj);
            }
        }
        return sadrzajSobe;
    }
   
   public void KreirajSadrzaj(SadrzajSobe sadrzaj)
   {
        if (File.Exists(fileLocation))
        {
            string novSadrzaj = ParseToString(sadrzaj);
            List<string> lista = new List<string>();
            lista.Add(novSadrzaj);
            File.AppendAllLines(fileLocation, lista);
        }
        else
        {

            MessageBox.Show("Nista");
        }
    }

    public string ParseToString(SadrzajSobe sadrzaj) {
        string tekst = sadrzaj.Soba.Id + "#" + sadrzaj.Predmet.Id + "#" + sadrzaj.Kolicina + "#";
        if (sadrzaj.DatumPremestanja.Year != 0001)
        {
            tekst = tekst + sadrzaj.DatumPremestanja.ToString() + "#";
        }
        if (sadrzaj.NovaSoba != null) { 
            tekst= tekst+sadrzaj.NovaSoba.Id; 
        }
        return tekst;
    }

   public void IzmeniSadrzaj(SadrzajSobe sadrzaj)
   {
        List<SadrzajSobe> listaSadrzaja = SviSadrzaji();
        foreach (SadrzajSobe s in listaSadrzaja) {
            if (s.Soba.Id.Equals(sadrzaj.Soba.Id) && s.Predmet.Id.Equals(sadrzaj.Predmet.Id)) {
                ObrisiSadrzaj(s);
                KreirajSadrzaj(sadrzaj);
                break;
            }
        }
   }
   
   public void ObrisiSadrzaj(SadrzajSobe sadrzaj)
   {
        if (File.Exists(fileLocation))
        {
            List<SadrzajSobe> listaSadrzaja = SviSadrzaji();
            List<string> tekst = new List<string>();
            foreach (SadrzajSobe s in listaSadrzaja)
            {
                if (!(s.Soba.Id.Equals(sadrzaj.Soba.Id)) || !(s.Predmet.Id.Equals(sadrzaj.Predmet.Id)))
                {
                    string linija = ParseToString(s);
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
   
   public string fileLocation = @"..\..\Datoteke\sadrzajiSoba.txt";

}