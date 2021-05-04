// File:    BazaLekova.cs
// Author:  Zola
// Created: Sunday, April 11, 2021 7:46:32 PM
// Purpose: Definition of Class BazaLekova

using System;
using System.Collections.Generic;
using System.IO;

public class BazaLekova
{
   public List<Lek> SviLekovi()
   {
        List<Lek> ret = new List<Lek>();
        if (File.Exists(@"..\..\Datoteke\lekovi.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\lekovi.txt");
            foreach (string line in lines)
            {
                Lek p = new Lek();
                string[] delovi = line.Split('#');
                p.Sifra = delovi[0];
                p.Ime = delovi[1];
                p.Opis = delovi[2];
                if (delovi[3].Equals("1"))
                {
                    p.PotrebanRecept = true;
                }
                else
                {
                    p.PotrebanRecept = false;
                }

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

                Console.WriteLine(line);
                ret.Add(p);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;
    }
   
   public void KreirajLek(Lek lek)
   {
        List<string> linije = new List<string>();
        string novaLinija =lek.Sifra + "#" + lek.Ime + "#" + lek.Opis + "#";
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
        novaLinija.Remove(novaLinija.Length - 1, 1);
        linije.Add(novaLinija);
        File.AppendAllLines(fileLocation, linije);
    }

    public void KreiraNovijLek(Lek lek)
    {
        List<string> linije = new List<string>();
        string novaLinija = lek.Sifra + "#" + lek.Ime + "#" + lek.Opis + "#";
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
        novaLinija.Remove(novaLinija.Length - 1, 1);
        novaLinija += System.Environment.NewLine;
        linije.Add(novaLinija);
        File.AppendAllLines(fileLocation, linije);
    }

    public void IzmeniLek(Lek lek)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiILek(Lek lekZaBrisanje)
   {
        List<string> linije = new List<string>();
        foreach (Lek lek in SviLekovi())
        {
            if (!lekZaBrisanje.Sifra.Equals(lek.Sifra))
            {
                string novaLinija =lek.Sifra + "#" + lek.Ime + "#" + lek.Opis + "#";
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
                linije.Add(novaLinija);
            }
        }
        File.WriteAllLines(fileLocation, linije);
    }
   
   public string fileLocation = @"..\..\Datoteke\lekovi.txt";

}