// File:    BazaLekara.cs
// Author:  Zola
// Created: Monday, March 22, 2021 8:08:26 PM
// Purpose: Definition of Class BazaLekara

using System;
using System.Collections.Generic;
using System.IO;

public class BazaLekara
{
    public List<Lekar> SviLekari()
    {
        List<Lekar> ret = new List<Lekar>();
        if (File.Exists(@"..\..\Datoteke\lekari.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\lekari.txt");
            foreach (string line in lines)
            {
                Lekar p = new Lekar(TipLekara.lekarSpecijalista, null);
                string[] delovi = line.Split('#');
                p.Jmbg = delovi[0];
                p.Ime = delovi[1];
                p.Prezime = delovi[2];
                if (delovi[3].Equals("0"))
                {
                    p.Tip = TipLekara.lekarOpstePrakse;
                }
                else
                {
                    p.Tip = TipLekara.lekarSpecijalista;
                }
                ret.Add(p);
                Console.WriteLine(line);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;
    }

    public void KreirajLekara(Lekar noviLekar)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiLekara(Lekar lekar)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniLekara(Lekar lekar)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}