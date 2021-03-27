// File:    BazaOperacija.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class BazaOperacija

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class BazaOperacija
{
    public List<Operacija> SveSledeceOperacije()
    {
        CultureInfo provider = new CultureInfo("en-UN");
        List<Operacija> ret = new List<Operacija>();
        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        BazaBolnica bazaBolnica = new BazaBolnica();
        List<Bolnica> bolnice = bazaBolnica.SveBolcine();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();
        if (File.Exists(@"..\..\Datoteke\operacije.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\operacije.txt");
            foreach (string line in lines)
            {
                Operacija o = new Operacija();
                string[] delovi = line.Split('#');
                Console.WriteLine(delovi[0]);
                o.VremePocetaOperacije = DateTime.ParseExact(delovi[0], "G", provider);
                o.VremeKrajaOperacije = DateTime.ParseExact(delovi[1], "G", provider);
                o.Pacijent.Jmbg = delovi[2];
                foreach (Pacijent p in pacijenti)
                {
                    if (o.Pacijent.Jmbg.Equals(p.Jmbg))
                    {
                        o.Pacijent.Prezime = p.Prezime;
                        o.Pacijent.Ime = p.Ime;
                        break;
                    }
                }

                foreach (Bolnica bolnica in bolnice)
                {
                    foreach (Soba s in bolnica.Soba)
                    {
                        if (delovi[4].Equals(s.Id))
                        {
                            o.Soba.Tip = s.Tip;
                        }
                    }
                }
                o.Lekar.Jmbg = delovi[3];
                o.Soba.Id = delovi[4];
                ret.Add(o);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;

    }

    public void ZakaziOperaciju(Operacija novaOperacija)
    {
        string linija = novaOperacija.VremePocetaOperacije.ToString() + "#" + novaOperacija.VremeKrajaOperacije.ToString() + "#" +
            novaOperacija.Pacijent.Jmbg + "#" + novaOperacija.Lekar.Jmbg + "#" + novaOperacija.Soba.Id + System.Environment.NewLine;
        File.AppendAllText(@"..\..\Datoteke\operacije.txt", linija);
    }

    public void OtkaziOperaciju(Operacija operacija)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniOperaciju(Operacija operacija)
   {
      throw new NotImplementedException();
   }
   
   public List<Operacija> OperacijeDatogPacijenta(Pacijent pacijent)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}