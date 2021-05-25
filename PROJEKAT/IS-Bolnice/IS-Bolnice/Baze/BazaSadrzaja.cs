// File:    BazaSadrzaja.cs
// Author:  teddy
// Created: Monday, April 26, 2021 3:42:44 PM
// Purpose: Definition of Class BazaSadrzaja

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

public class BazaSadrzaja
{

    private static string vremenskiFormatPisanje = "M/d/yyyy";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy",
        "M-d-yyyy"
    };

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

    public bool PostojiOpremaUBolnici(string idPredmeta) {
        List<SadrzajSobe> sadrzajSoba = SviSadrzaji();
        foreach (SadrzajSobe sadrzaj in sadrzajSoba) {
            if (sadrzaj.Predmet.Id.Equals(idPredmeta)) {
                return true;
            }
        }
        return false;
    }

    public int BrojKrevetaUSobi(string idSobe)
    {
        List<SadrzajSobe> operemaUSObi = GetSadrzajSobe(idSobe);
        foreach (SadrzajSobe predmet in operemaUSObi)
        {
            if (predmet.Predmet.Id.Equals("1111"))
            {
                return predmet.Kolicina;
            }
        }

        return 0;
    }

    public SadrzajSobe ParseFromString(string line) {

        string[] niz = line.Split('#');
        Soba soba = new Soba();
        soba.Id = niz[0];
        BazaOpreme bazaOpreme = new BazaOpreme();
        Predmet predmet = bazaOpreme.GetPredmet(niz[1]);
        //predmet.Id = niz[1];
        //predmet.Naziv = new BazaOpreme().GetPredmet(predmet.Id).Naziv;
        Soba novaSoba = new Soba();
        SadrzajSobe s;
        if (!niz[3].Equals(""))
        {
            novaSoba.Id = niz[4];
            s = new SadrzajSobe(soba, predmet, Int32.Parse(niz[2]), DateTime.ParseExact(niz[3], vremenskiFormatiCitanje, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), novaSoba);
        }
        else {
            s = new SadrzajSobe(soba, predmet, Int32.Parse(niz[2]));
        }
        return s;
    }

    private List<SadrzajSobe> SadrzajiUPrenosu()
    {
        List<SadrzajSobe> sviSadrzaji = SviSadrzaji();
        List<SadrzajSobe> sadrzajiUPrenosu = new List<SadrzajSobe>();
        foreach (SadrzajSobe sadrzaj in sviSadrzaji)
        {
            if (sadrzaj.NovaSoba != null)
            {
                sadrzajiUPrenosu.Add(sadrzaj);
            }
        }
        return sadrzajiUPrenosu;
    }

    public void IzvrsiTransport()
    {
        List<SadrzajSobe> sadrzaji = SadrzajiUPrenosu();
        foreach (SadrzajSobe sadrzaj in sadrzaji)
        {
            if (sadrzaj.DatumPremestanja < DateTime.UtcNow)
            {
                if (!DodajOpremuNaStanje(sadrzaj))
                {
                    SadrzajSobe noviSadrzaj = new SadrzajSobe(sadrzaj.NovaSoba.Id, sadrzaj.Predmet.Id, sadrzaj.Kolicina);
                    KreirajSadrzaj(noviSadrzaj);
                }
                ObrisiSadrzaj(sadrzaj);

            }
        }
    }

    private bool DodajOpremuNaStanje(SadrzajSobe sadrzajUPrenosu)
    {
        bool opremaPostojiUProstoriji = false;
        List<SadrzajSobe> novaSoba = GetSadrzajSobe(sadrzajUPrenosu.NovaSoba.Id);
        foreach (SadrzajSobe sadrzaj in novaSoba)
        {
            if (sadrzaj.Predmet.Id.Equals(sadrzajUPrenosu.Predmet.Id))
            {
                sadrzaj.Kolicina += sadrzajUPrenosu.Kolicina;
                IzmeniSadrzaj(sadrzaj);
                opremaPostojiUProstoriji = true;
                break;
            }

        }
        return opremaPostojiUProstoriji;
    }

    public List<SadrzajSobe> GetSadrzajSobe(string idSobe) {

        List<SadrzajSobe> sadrzajSobe = new List<SadrzajSobe>();
        List<SadrzajSobe> sadrzajSvihSoba = SviSadrzaji() ;
        foreach (SadrzajSobe sadrzaj in sadrzajSvihSoba) {
            if (sadrzaj.Soba.Id.Equals(idSobe) && sadrzaj.NovaSoba == null) {
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
            tekst = tekst + sadrzaj.DatumPremestanja.ToString(vremenskiFormatPisanje) + "#";
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
            if (s.Soba.Id.Equals(sadrzaj.Soba.Id) && s.Predmet.Id.Equals(sadrzaj.Predmet.Id) && s.NovaSoba == null) {
                ObrisiSadrzaj(s);
                if (sadrzaj.Kolicina > 0)
                {
                    KreirajSadrzaj(sadrzaj);
                }
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