// File:    BazaUpravnika.cs
// Author:  Zola
// Created: Monday, March 22, 2021 9:03:16 PM
// Purpose: Definition of Class BazaUpravnika

using System;
using System.Collections.Generic;
using System.IO;

public class BazaUpravnika
{
   public List<Upravnik> SviUpravnici()
   {
        string[] linije = File.ReadAllLines(fileLocation);

        List<Upravnik> upravnici = new List<Upravnik>();

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split('#');

            Upravnik u = new Upravnik();
            u.Jmbg = delovi[0];
            u.KorisnickoIme = delovi[1];
            u.Sifra = delovi[2];
            u.Ime = delovi[3];
            u.Prezime = delovi[4];
            u.BrojTelefona = delovi[5];
            u.EMail = delovi[6];
            u.Adresa = delovi[7];
            if (delovi[8].Equals("muski"))
            {
                u.Pol = Pol.muski;
            }
            else if (delovi[8].Equals("zenski"))
            {
                u.Pol = Pol.zenski;
            }
            else
            {
                u.Pol = Pol.drugo;
            }

            upravnici.Add(u);
        }
        return upravnici;
    }
   
   public void KreirajUpravnika(Upravnik noviUpravnik)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiUpravnika(Upravnik upravnik)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniUpravnika(Upravnik upravnik)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation = @"..\..\Datoteke\upravnici.txt";

}