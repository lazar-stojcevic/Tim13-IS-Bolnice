// File:    OdgovorNaZahtevFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:32:38 PM
// Purpose: Definition of Class OdgovorNaZahtevFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;

public class OdgovorNaZahtevFajlRepozitorijum : GenerickiFajlRepozitorijum<OdgovorNaZahtevZaValidaciju>, IOdgovorNaZahtevRepozitorijum
{
    public OdgovorNaZahtevFajlRepozitorijum():base(@"..\..\Datoteke\odgovorLekovi.txt")
   {

   }
    public override OdgovorNaZahtevZaValidaciju KreirajEntitet(string[] podaciEntiteta)
    {
        
            bool potrebanRecept;
            if (podaciEntiteta[3].ToString().Equals("False"))
            {
                potrebanRecept = false;
            }
            else
            {
                potrebanRecept = true;
            }
            Lek lek = new Lek(podaciEntiteta[0].ToString(), podaciEntiteta[1].ToString(), podaciEntiteta[2].ToString(), potrebanRecept);
            lek.Alergeni = ParseStringToSastojci(podaciEntiteta[4].ToString());
            lek.ZamenskiLekovi = ParseStringToZamenskeLekove(podaciEntiteta[5].ToString());
            OdgovorNaZahtevZaValidaciju odgovorNaZahtevZaValidaciju = new OdgovorNaZahtevZaValidaciju(lek, podaciEntiteta[6].ToString());

            return odgovorNaZahtevZaValidaciju;
    }

    public override string KreirajTextZaUpis(OdgovorNaZahtevZaValidaciju entitet)
    {
        
            string novaLinija = entitet.Id + "#" + entitet.Lek.Ime + "#" + entitet.Lek.Opis + "#" + entitet.Lek.PotrebanRecept + "#";
            if (entitet.Lek.Alergeni.Count != 0)
            {
                foreach (Sastojak sastojak in entitet.Lek.Alergeni)
                {
                    novaLinija += sastojak.Ime + ",";
                }
            }
            novaLinija += "#";
            if (entitet.Lek.ZamenskiLekovi.Count != 0)
            {
                foreach (Lek zamenskiLek in entitet.Lek.ZamenskiLekovi)
                {
                    novaLinija = novaLinija + zamenskiLek.Id + "/";
                }
                novaLinija = novaLinija.Remove(novaLinija.Length - 1);
            }
           return novaLinija += "#" + entitet.Obrazlozenje;
        
    }

    public List<Lek> ParseStringToZamenskeLekove(string tekst)
    {
        List<Lek> noviLekovi = new List<Lek>();
        string[] lekovi = tekst.Split('/');
        foreach (string lek in lekovi)
        {
            if (!lek.Equals(""))
            {
                Lek noviLek = new Lek(lek);
                noviLekovi.Add(noviLek);
            }
        }
        return noviLekovi;

    }

    public List<Sastojak> ParseStringToSastojci(string tekst)
    {
        string[] sastojci = tekst.Split(',');
        List<Sastojak> noviSastojci = new List<Sastojak>();
        foreach (string sastojak in sastojci)
        {
            if (!sastojak.Equals(""))
            {
                Sastojak noviSastojak = new Sastojak(sastojak);
                noviSastojci.Add(noviSastojak);
            }
        }
        return noviSastojci;
    }

    /*
    public List<OdgovorNaZahtevZaValidaciju> SviOdgovoriNaZahteve()
    {
         if (File.Exists(fileLocation))
         {
             return ParseStringToOdgovor(File.ReadAllLines(fileLocation));
         }
         else
         {
             MessageBox.Show("Datoteka nije pronadjena!");
             return new List<OdgovorNaZahtevZaValidaciju>();
         }
     }

     public OdgovorNaZahtevZaValidaciju GetOdgovor(string sifra)
     {
         List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve = SviOdgovoriNaZahteve();
         foreach (OdgovorNaZahtevZaValidaciju odgovorNaZahtev in odgovoriNaZahteve)
         {
             if (odgovorNaZahtev.Lek.Sifra.Equals(sifra))
             {
                 return odgovorNaZahtev;
             }
         }
         return new OdgovorNaZahtevZaValidaciju();
     }

     public List<OdgovorNaZahtevZaValidaciju> ParseStringToOdgovor(string[] tekst)
     {
         List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve = new List<OdgovorNaZahtevZaValidaciju>();
         foreach (string linije in tekst)
         {
             string[] linija = linije.Split('#');
             bool potrebanRecept;
             if (linija[3].ToString().Equals("False"))
             {
                 potrebanRecept = false;
             }
             else
             {
                 potrebanRecept = true;
             }
             Lek lek = new Lek(linija[0].ToString(), linija[1].ToString(), linija[2].ToString(), potrebanRecept);
             lek.Alergeni = ParseStringToSastojci(linija[4].ToString());
             lek.ZamenskiLekovi = ParseStringToZamenskeLekove(linija[5].ToString());
             OdgovorNaZahtevZaValidaciju odgovorNaZahtevZaValidaciju = new OdgovorNaZahtevZaValidaciju(lek, linija[6].ToString());
             odgovoriNaZahteve.Add(odgovorNaZahtevZaValidaciju);
         }
         return odgovoriNaZahteve;
     }

     public List<Lek> ParseStringToZamenskeLekove(string tekst)
     {
         List<Lek> noviLekovi = new List<Lek>();
         string[] lekovi = tekst.Split('/');
         foreach (string lek in lekovi)
         {
             if (!lek.Equals(""))
             {
                 Lek noviLek = new Lek(lek);
                 noviLekovi.Add(noviLek);
             }
         }
         return noviLekovi;

     }

     public List<Sastojak> ParseStringToSastojci(string tekst)
     {
         string[] sastojci = tekst.Split(',');
         List<Sastojak> noviSastojci = new List<Sastojak>();
         foreach (string sastojak in sastojci)
         {
             if (!sastojak.Equals(""))
             {
                 Sastojak noviSastojak = new Sastojak(sastojak);
                 noviSastojci.Add(noviSastojak);
             }
         }
         return noviSastojci;
     }
     //Pogledaj donju funkciju i vidi da li ona vise odgovara. Dodala sam u odgovor polje lek pa moze sve sa jednim parametrom
     public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
    {
         string novaLinija = zahtev.Lek.Sifra + "#" + zahtev.Lek.Ime + "#" + zahtev.Lek.Opis + "#" + zahtev.Lek.PotrebanRecept + "#";
         if (zahtev.Lek.Alergeni.Count != 0)
         {
             foreach (Sastojak sastojak in zahtev.Lek.Alergeni)
             {
                 novaLinija += sastojak.Ime + ",";
             }
         }

         novaLinija += "#";

         //JOVANA DODAJ KAD UBACIS ZAMENSKE LEKOVI-OKEJ
         if (zahtev.Lek.ZamenskiLekovi.Count != 0)
         {
             foreach (Lek zamenskiLek in zahtev.Lek.ZamenskiLekovi)
             {
                 novaLinija = novaLinija + zamenskiLek.Sifra + "/";
             }
             novaLinija = novaLinija.Remove(novaLinija.Length - 1);
         }
         novaLinija += "#" + odgovor.Obrazlozenje;

         File.AppendAllText(fileLocation, novaLinija);
    }





     public List<string> ParseToString(List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve)
     {
         List<string> tekst = new List<string>();
         foreach (OdgovorNaZahtevZaValidaciju odgovor in odgovoriNaZahteve)
         {
             string novaLinija = odgovor.Lek.Sifra + "#" + odgovor.Lek.Ime + "#" + odgovor.Lek.Opis + "#" + odgovor.Lek.PotrebanRecept + "#";
             if (odgovor.Lek.Alergeni.Count != 0)
             {
                 foreach (Sastojak sastojak in odgovor.Lek.Alergeni)
                 {
                     novaLinija += sastojak.Ime + ",";
                 }
             }
             novaLinija += "#";
             if (odgovor.Lek.ZamenskiLekovi.Count != 0)
             {
                 foreach (Lek zamenskiLek in odgovor.Lek.ZamenskiLekovi)
                 {
                     novaLinija = novaLinija + zamenskiLek.Sifra + "/";
                 }
                 novaLinija = novaLinija.Remove(novaLinija.Length - 1);
             }
             novaLinija += "#" + odgovor.Obrazlozenje;
             tekst.Add(novaLinija);
         }
         return tekst;
     }

     public void ObrisiOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor)
    {
         List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve = new List<OdgovorNaZahtevZaValidaciju>();
         foreach (OdgovorNaZahtevZaValidaciju odgovorNaZahtev in SviOdgovoriNaZahteve())
         {
             if (!odgovorNaZahtev.Lek.Sifra.Equals(odgovor.Lek.Sifra))
             {
                 odgovoriNaZahteve.Add(odgovorNaZahtev);
             }
         }
         File.WriteAllLines(fileLocation, ParseToString(odgovoriNaZahteve));
     }

     */

}