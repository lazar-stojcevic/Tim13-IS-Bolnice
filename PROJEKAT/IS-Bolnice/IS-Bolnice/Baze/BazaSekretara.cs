// File:    BazaSekretara.cs
// Author:  Zola
// Created: Monday, March 22, 2021 8:20:54 PM
// Purpose: Definition of Class BazaSekretara

using System;
using System.Collections.Generic;
using System.IO;

public class BazaSekretara
{
   public List<Sekretar> SviSekretari()
   {
        string[] linije = File.ReadAllLines(fileLocation);

        List<Sekretar> sekretari = new List<Sekretar>();

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split('#');

            Sekretar s = new Sekretar();
            s.Jmbg = delovi[0];
            s.KorisnickoIme = delovi[1];
            s.Sifra = delovi[2];
            s.Ime = delovi[3];
            s.Prezime = delovi[4];
            s.BrojTelefona = delovi[5];
            s.EMail = delovi[6];
            s.Adresa = delovi[7];
            if (delovi[8].Equals("muski"))
            {
                s.Pol = Pol.muski;
            }
            else if (delovi[8].Equals("zenski"))
            {
                s.Pol = Pol.zenski;
            }
            else
            {
                s.Pol = Pol.drugo;
            }

            sekretari.Add(s);
        }
        return sekretari;
    }
   
   public void KreirajSekretara(Sekretar noviSekretar)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiSekretara(Sekretar sekretar)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniSekretara(Sekretar sekretar)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation = @"..\..\Datoteke\sekretari.txt";

}