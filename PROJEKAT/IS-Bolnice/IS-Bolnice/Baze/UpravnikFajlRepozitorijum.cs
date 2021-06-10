// File:    UpravnikFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, March 22, 2021 9:03:16 PM
// Purpose: Definition of Class UpravnikFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class UpravnikFajlRepozitorijum : GenerickiFajlRepozitorijum<Upravnik>, IUpravnikRepozitorijum
{
    public UpravnikFajlRepozitorijum() : base(@"..\..\Datoteke\upravnici.txt")
    {
    }

    public override Upravnik KreirajEntitet(string[] podaciEntiteta)
    {
        Upravnik u = new Upravnik();
        u.Jmbg = podaciEntiteta[0];
        u.Id = podaciEntiteta[0];
        u.KorisnickoIme = podaciEntiteta[1];
        u.Sifra = podaciEntiteta[2];
        u.Ime = podaciEntiteta[3];
        u.Prezime = podaciEntiteta[4];
        u.BrojTelefona = podaciEntiteta[5];
        u.EMail = podaciEntiteta[6];
        u.Adresa = podaciEntiteta[7];
        if (podaciEntiteta[8].Equals("muski"))
        {
            u.Pol = Pol.muski;
        }
        else if (podaciEntiteta[8].Equals("zenski"))
        {
            u.Pol = Pol.zenski;
        }
        else
        {
            u.Pol = Pol.drugo;
        }

        return u;
    }

    public override string KreirajTextZaUpis(Upravnik entitet)
    {
        throw new NotImplementedException();
    }
}