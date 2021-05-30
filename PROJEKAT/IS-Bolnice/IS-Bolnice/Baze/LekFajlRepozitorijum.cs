// File:    LekFajlRepozitorijum.cs
// Author:  Zola
// Created: Sunday, April 11, 2021 7:46:32 PM
// Purpose: Definition of Class LekFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class LekFajlRepozitorijum: GenerickiFajlRepozitorijum<Lek>, ILekRepozitorijum
{
    public LekFajlRepozitorijum() : base(@"..\..\Datoteke\lekovi.txt")
    {

    }
    /*
   public List<Lek> SviLekovi()
   {
        List<Lek> ret = new List<Lek>();
        KreirajLekove(ret);
       
        return ret;
    }

   private static void KreirajLekove(List<Lek> ret)
   {
       if (File.Exists(@"..\..\Datoteke\lekovi.txt"))
       {
           string[] lines = File.ReadAllLines(@"..\..\Datoteke\lekovi.txt");
           foreach (string line in lines)
           {
               Lek lek = new Lek();
               string[] delovi = line.Split('#');

               lek.Id = delovi[0];
               lek.Ime = delovi[1];
               lek.Opis = delovi[2];

               PostaviDaLiJePotrebanRecept(delovi, lek);

               KreirajAlergene(delovi, lek);

               KreirajZamenskeLekove(delovi, lek);

               Console.WriteLine(line);
               ret.Add(lek);
           }
       }
   }

   private static void PostaviDaLiJePotrebanRecept(string[] delovi, Lek p)
   {
       if (delovi[3].Equals("1"))
       {
           p.PotrebanRecept = true;
       }
       else
       {
           p.PotrebanRecept = false;
       }
   }

   private static void KreirajAlergene(string[] delovi, Lek p)
   {
       string alergeniSvi = delovi[4];
       if (!alergeniSvi.Equals(""))
       {
           string[] alergen = alergeniSvi.Split(',');
           foreach (string a in alergen)
           {
               if (!a.Equals(""))
               {
                   Sastojak s = new Sastojak(a);
                   p.Alergeni.Add(s);
               }
           }
       }
   }

   private static void KreirajZamenskeLekove(string[] delovi, Lek p)
   {
       string zamenskiLekoviSvi = delovi[5];
       if (!zamenskiLekoviSvi.Equals(""))
       {
           string[] zameskiLek = zamenskiLekoviSvi.Split('/');
           foreach (string deo in zameskiLek)
           {
               if (deo.Equals("")) continue;
               Lek lek = new Lek(deo);
               p.ZamenskiLekovi.Add(lek);
           }
       }
   }

   public Lek GetLek(string sifraLeka)
    {
        List<Lek> lekovi = SviLekovi();
        foreach (Lek lek in lekovi)
        {
            if (lek.Id.Equals(sifraLeka))
            {
                return lek;
            }
        }
        return new Lek();
    }

    public void KreirajLek(Lek lek)
   {
       List<string> linije = new List<string>();
       string novaLinija = lek.Id + "#" + lek.Ime + "#" + lek.Opis + "#";
       if (lek.PotrebanRecept)
       {
           novaLinija += "1#";
       }
       else novaLinija += "0#";


       if (lek.Alergeni.Count != 0)
       {
           foreach (Sastojak sastojak in lek.Alergeni)
           {
               novaLinija += sastojak.Ime + ",";
           }
       }
       else
       {
           novaLinija += "nema,";
       }

       novaLinija += "#";
       if (lek.ZamenskiLekovi.Count != 0)
       {
           foreach (Lek lekIter in lek.ZamenskiLekovi)
           {
               novaLinija += lekIter.Id + "/";
           }
       }

       novaLinija.Remove(novaLinija.Length - 1, 1);
       linije.Add(novaLinija);
       File.AppendAllLines(fileLocation, linije);
    }

    public void KreiraNoviLek(Lek lek)
    {
        string novaLinija = System.Environment.NewLine + lek.Sifra + "#" + lek.Ime + "#" + lek.Opis + "#";
        if (lek.PotrebanRecept)
        {
            novaLinija += "1#";
        }
        else novaLinija += "0#";


        if (lek.Alergeni.Count != 0)
        {
            foreach (Sastojak sastojak in lek.Alergeni)
            {
                novaLinija += sastojak.Ime + ",";
            }
        }
        else
        {
            novaLinija += "nema,";
        }
        novaLinija = novaLinija.Remove(novaLinija.Length - 1);

        novaLinija = novaLinija + "#";
        if (lek.ZamenskiLekovi.Count != 0)
        {
            foreach (Lek zamenskiLek in lek.ZamenskiLekovi)
            {
                novaLinija = novaLinija + zamenskiLek.Sifra + "/";
            }
            novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        }
        File.AppendAllText(fileLocation, novaLinija);

    }

    public void IzmeniLek(Lek lek)
   {
        ObrisiILek(lek);
        KreirajLek(lek);
    }
   
   public void ObrisiILek(Lek lekZaBrisanje)
   {
        List<string> linije = new List<string>();
        foreach (Lek lek in SviLekovi())
        {
            if (!lekZaBrisanje.Id.Equals(lek.Id))
            {
                string novaLinija =lek.Id + "#" + lek.Ime + "#" + lek.Opis + "#";
                if (lek.PotrebanRecept)
                {
                    novaLinija += "1#";
                }
                else novaLinija += "0#";


                if (lek.Alergeni.Count != 0)
                {
                    foreach (Sastojak sastojak in lek.Alergeni)
                    {
                        novaLinija += sastojak.Ime + ",";
                    }
                }
                else
                {
                    novaLinija += "nema,";
                }
                novaLinija = novaLinija.Remove(novaLinija.LastIndexOf(','), 1);
                novaLinija = novaLinija + "#";
                if (lek.ZamenskiLekovi.Count != 0)
                {
                    foreach (Lek zamenskiLek in lek.ZamenskiLekovi)
                    {
                        novaLinija = novaLinija + zamenskiLek.Id + "/";
                    }
                    novaLinija = novaLinija.Remove(novaLinija.Length - 1);
                }


                linije.Add(novaLinija);
            }
        }
        File.WriteAllLines(fileLocation, linije);
    }
    */
    public override Lek KreirajEntitet(string[] podaciEntiteta)
    {
        Lek lek = new Lek();

        lek.Id = podaciEntiteta[0];
        lek.Ime = podaciEntiteta[1];
        lek.Opis = podaciEntiteta[2];

        PostaviDaLiJePotrebanRecept(podaciEntiteta, lek);

        KreirajAlergene(podaciEntiteta, lek);

        KreirajZamenskeLekove(podaciEntiteta, lek);

        return lek;
    }

    public override string KreirajTextZaUpis(Lek lek)
    {
        string novaLinija = System.Environment.NewLine + lek.Id + "#" + lek.Ime + "#" + lek.Opis + "#";
        if (lek.PotrebanRecept)
        {
            novaLinija += "1#";
        }
        else novaLinija += "0#";


        if (lek.Alergeni.Count != 0)
        {
            foreach (Sastojak sastojak in lek.Alergeni)
            {
                novaLinija += sastojak.Ime + ",";
            }
        }
        else
        {
            novaLinija += "nema,";
        }
        novaLinija = novaLinija.Remove(novaLinija.Length - 1);

        novaLinija = novaLinija + "#";
        if (lek.ZamenskiLekovi.Count != 0)
        {
            foreach (Lek zamenskiLek in lek.ZamenskiLekovi)
            {
                novaLinija = novaLinija + zamenskiLek.Id + "/";
            }
            novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        }

        return novaLinija;
    }

    private static void PostaviDaLiJePotrebanRecept(string[] delovi, Lek p)
    {
        if (delovi[3].Equals("1"))
        {
            p.PotrebanRecept = true;
        }
        else
        {
            p.PotrebanRecept = false;
        }
    }

    private static void KreirajAlergene(string[] delovi, Lek p)
    {
        string alergeniSvi = delovi[4];
        if (!alergeniSvi.Equals(""))
        {
            string[] alergen = alergeniSvi.Split(',');
            foreach (string a in alergen)
            {
                if (!a.Equals(""))
                {
                    Sastojak s = new Sastojak(a);
                    p.Alergeni.Add(s);
                }
            }
        }
    }

    private static void KreirajZamenskeLekove(string[] delovi, Lek p)
    {
        string zamenskiLekoviSvi = delovi[5];
        if (!zamenskiLekoviSvi.Equals(""))
        {
            string[] zameskiLek = zamenskiLekoviSvi.Split('/');
            foreach (string deo in zameskiLek)
            {
                if (deo.Equals("")) continue;
                Lek lek = new Lek(deo);
                p.ZamenskiLekovi.Add(lek);
            }
        }
    }
    public string fileLocation = @"..\..\Datoteke\lekovi.txt";

}