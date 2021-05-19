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
                Bolnica b = ParseStringToBolnica(line);
                ret.Add(b);
            }
        }
        else
        {

            MessageBox.Show("Nista");
        }
        return ret;

    }

    public Bolnica GetBolnica() {
        foreach (Bolnica bolnicaIter in SveBolnice()) {
            return bolnicaIter;
        }
        return new Bolnica();
    }

    public Bolnica ParseStringToBolnica(string podaciOSobi)
    {
        Bolnica b = new Bolnica();
        string[] delovi = podaciOSobi.Split('#');
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
            s.Zauzeta = ParseStringToBool(ds[1]);
            s.Tip = (RoomType)Enum.Parse(typeof(RoomType), ds[2]);
            s.Obrisano = ParseStringToBool(ds[3]);
            s.Sprat = Int32.Parse(ds[4]);
            s.Kvadratura = Double.Parse(ds[5]);
            b.AddSoba(s);
        }
        return b;
    }

    public bool ParseStringToBool(string tekst) {
        if (tekst.Equals("False"))
        {
           return false;
        }
        else
        {
            return true;
        }
    }

    public List<Soba> GetSobe()
    {
        List<Bolnica> bolnice = SveBolnice();
        foreach (Bolnica b in bolnice)
        {
            return b.Soba;
        }
        List<Soba> sobe = new List<Soba>();
        return sobe;
    }

    public Soba GetMagacin()
    {
        Soba soba = new Soba();
        List<Bolnica> bolnice = SveBolnice();
        foreach (Bolnica b in bolnice)
        {
            foreach (Soba s in b.Soba)
            {
                if (s.Tip == RoomType.magacin)
                {
                    soba = s;
                    break;
                }
            }
        }

        return soba;
    }

    public Soba GetSobaById(string idSobe) {
        List<Soba> sobe = GetSobe();
        foreach (Soba soba in sobe) {
            if (soba.Id.Equals(idSobe)) {
                return soba;
            }
        }
        return new Soba();
    }

    public void KreirajBolnicu(Bolnica novaBolnica)
    {

        if (File.Exists(fileLocation))
        {
            string[] niz = new string[1];
            niz[0] = novaBolnica.Ime + "#" + novaBolnica.Adresa + "#" + novaBolnica.EMail + "#" + novaBolnica.BrojTelefona + "#";
            foreach (Soba s in novaBolnica.Soba)
            {
                niz[0] = niz[0] + s.Id + "/" + s.Zauzeta + "/" + s.Tip + "/" + s.Obrisano + "/" + s.Sprat + "/" + s.Kvadratura + "%";
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

    public List<Soba> SveOperacioneSaleOveBolnice()
    {
        List<Bolnica> bolnice = SveBolnice();
        List<Soba> sveSobe = bolnice[0].Soba; // za sada se podrazumeva da postoji samo jedna bolnica
        List<Soba> operacioneSale = new List<Soba>();

        foreach (Soba soba in sveSobe)
        {
            if (soba.Tip.Equals(RoomType.operacionaSala) && !soba.Obrisano && !soba.TrenutnoPodRenoviranjem())
            {
                operacioneSale.Add(soba);
            }
        }
        return operacioneSale;
    }
   
   public string fileLocation = @"..\..\Datoteke\bolnice.txt";

}