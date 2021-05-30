// File:    SekretarFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, March 22, 2021 8:20:54 PM
// Purpose: Definition of Class SekretarFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class SekretarFajlRepozitorijum : GenerickiFajlRepozitorijum<Sekretar>, ISekretarRepozitorijum
{
    public SekretarFajlRepozitorijum() : base(@"..\..\Datoteke\sekretari.txt")
    {
    }

    public override Sekretar KreirajEntitet(string[] podaciEntiteta)
    {
        Sekretar s = new Sekretar();
        s.Jmbg = podaciEntiteta[0];
        s.KorisnickoIme = podaciEntiteta[1];
        s.Sifra = podaciEntiteta[2];
        s.Ime = podaciEntiteta[3];
        s.Prezime = podaciEntiteta[4];
        s.BrojTelefona = podaciEntiteta[5];
        s.EMail = podaciEntiteta[6];
        s.Adresa = podaciEntiteta[7];
        if (podaciEntiteta[8].Equals("muski"))
        {
            s.Pol = Pol.muski;
        }
        else if (podaciEntiteta[8].Equals("zenski"))
        {
            s.Pol = Pol.zenski;
        }
        else
        {
            s.Pol = Pol.drugo;
        }

        return s;
    }

    public override string KreirajTextZaUpis(Sekretar entitet)
    {
        throw new NotImplementedException();
    }
}