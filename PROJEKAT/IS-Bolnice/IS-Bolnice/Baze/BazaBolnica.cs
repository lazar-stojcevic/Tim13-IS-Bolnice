// File:    BazaBolnica.cs
// Author:  Zola
// Created: Monday, March 22, 2021 9:11:00 PM
// Purpose: Definition of Class BazaBolnica

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class BazaBolnica
{
    public List<Bolnica> SveBolnice()
    {
        List<Bolnica> ret = new List<Bolnica>();
        if (File.Exists(fileLocation))
        {
            string[] lines = File.ReadAllLines(fileLocation);
            foreach (string line in lines)
            {
                Bolnica b = new Bolnica();
                string[] delovi = line.Split('#');
                b.Ime = delovi[0];
                b.Adresa = delovi[1];
                b.EMail = delovi[2];
                b.BrojTelefona = delovi[3];
                string[] sobe = delovi[4].Split('%');
                foreach (string linije in sobe)
                {
                    if (linije.Equals("")) break;
                    Soba s = new Soba();
                    string[] ds = linije.Split('/');
                    s.Id = ds[0];
                    if (ds[1] == "False")
                    {
                        s.Zauzeta = false;
                    }
                    else
                    {
                        s.Zauzeta = true;
                    }
                    if (ds[2] == "False")
                    {
                        s.PodRenoviranje = false;
                    }
                    else
                    {
                        s.PodRenoviranje = true;
                    }
                    s.Tip = (RoomType)Enum.Parse(typeof(RoomType), ds[3]);
                    if (ds[4] == "False")
                    {
                        s.Obrisano = false;
                    }
                    else
                    {
                        s.Obrisano = true;
                    }
                    s.Sprat = Int32.Parse(ds[5]);
                    s.Kvadratura = Double.Parse(ds[6]);
                    b.AddSoba(s);
                }

                ret.Add(b);
            }
        }
        else
        {

            MessageBox.Show("Nista");
        }
        return ret;
    }

    public void KreirajBolnicu(Bolnica novaBolnica)
    {

        if (File.Exists(fileLocation))
        {
            string[] niz = new string[1];
            niz[0] = novaBolnica.Ime + "#" + novaBolnica.Adresa + "#" + novaBolnica.EMail + "#" + novaBolnica.BrojTelefona + "#";
            foreach (Soba s in novaBolnica.Soba)
            {
                niz[0] = niz[0] + s.Id + "/" + s.Zauzeta + "/" + s.PodRenoviranje + "/" + s.Tip + "/" + s.Obrisano + "/" + s.Sprat + "/" + s.Kvadratura + "%";
            }
            niz[0] = niz[0] + "#";
            File.WriteAllLines(fileLocation, niz);
        }
        else
        {

            MessageBox.Show("Nista");
        }
    }

    public void ObrisiBolnicu(Bolnica bolnica)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniBolnicu(Bolnica bolnica)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation = @"..\..\Datoteke\bolnice.txt";

}