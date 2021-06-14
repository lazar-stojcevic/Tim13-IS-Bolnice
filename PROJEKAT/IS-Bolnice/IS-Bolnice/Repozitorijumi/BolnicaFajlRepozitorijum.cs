// File:    BolnicaFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, March 22, 2021 9:11:00 PM
// Purpose: Definition of Class BolnicaFajlRepozitorijum

using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class BolnicaFajlRepozitorijum: GenerickiFajlRepozitorijum<Bolnica>, IBolnicaRepozitorijum
{

    public BolnicaFajlRepozitorijum() : base(@"..\..\Datoteke\bolnice.txt") { }

   
    public Bolnica GetBolnica() {
        foreach (Bolnica bolnicaIter in GetSve()) {
            return bolnicaIter;
        }
        return new Bolnica();
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
        List<Bolnica> bolnice = GetSve();
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
        List<Bolnica> bolnice = GetSve();
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

    public List<Soba> GetSveSobeZaPregled()
    {
        List<Soba> sveSobeZaPregled = new List<Soba>();
        foreach (Soba soba in GetSobe())
        {
            if (!soba.Tip.Equals(RoomType.magacin))
            {
                sveSobeZaPregled.Add(soba);
            }
        }
        return sveSobeZaPregled;
    }

    public List<Soba> GetSveSobeZaHospitalizaciju()
    {
        List<Soba> sveSobeZAHospitalizaciju = new List<Soba>();
        foreach (Soba soba in GetSobe())
        {
            if (soba.Tip.Equals(RoomType.bolnickaSoba))
            {
                sveSobeZAHospitalizaciju.Add(soba);
            }
        }
        return sveSobeZAHospitalizaciju;
    }
    public List<Soba> GetSveOperacioneSale()
    {
        List<Soba> sveOperacioneSale = new List<Soba>();
        foreach (Soba soba in GetSobe())
        {
            if (soba.Tip.Equals(RoomType.operacionaSala))
            {
                sveOperacioneSale.Add(soba);
            }
        }
        return sveOperacioneSale;
    }

    public override Bolnica KreirajEntitet(string[] podaciEntiteta)
    {
        Bolnica b = new Bolnica();
        b.Ime = podaciEntiteta[0];
        b.Adresa = podaciEntiteta[1];
        b.EMail = podaciEntiteta[2];
        b.BrojTelefona = podaciEntiteta[3];
        string[] sobe = podaciEntiteta[4].Split('%');
        foreach (string linije in sobe)
        {
            if (linije.Equals("")) break;
            Soba s = new Soba();
            string[] ds = linije.Split('/');
            s.Id = ds[0];
            s.Tip = (RoomType)Enum.Parse(typeof(RoomType), ds[1]);
            s.Obrisano = ParseStringToBool(ds[2]);
            s.Sprat = Int32.Parse(ds[3]);
            s.Kvadratura = Double.Parse(ds[4]);
            b.AddSoba(s);
        }
        return b;
    }

    public override string KreirajTextZaUpis(Bolnica entitet)
    {
        string linija= entitet.Ime + "#" + entitet.Adresa + "#" + entitet.EMail + "#" + entitet.BrojTelefona + "#";
        foreach (Soba s in entitet.Soba)
        {
            linija = linija + s.Id  + "/" + s.Tip + "/" + s.Obrisano + "/" + s.Sprat + "/" + s.Kvadratura + "%";
        }
        linija = linija + "#";
        return linija;
    }


}