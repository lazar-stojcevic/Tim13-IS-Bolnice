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

                string alergeniSvi = delovi[3];
                if (!alergeniSvi.Equals("")) 
                 {
                    string[] alergen = alergeniSvi.Split(',');
                    foreach (string a in alergen)
                    {
                        if (!a.Equals(""))
                        {
                            p.Alergeni.Add(a);
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
      throw new NotImplementedException();
   }
   
   public void IzmeniLek(Lek lek)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiILek(Lek lek)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}