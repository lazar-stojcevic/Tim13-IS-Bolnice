// File:    BazaOpreme.cs
// Author:  teddy
// Created: Monday, April 12, 2021 6:08:22 PM
// Purpose: Definition of Class BazaOpreme

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class BazaOpreme : GenerickiFajlRepozitorijum<Predmet>, OpremaRepozitorijum
{
    public BazaOpreme():base(@"..\..\Datoteke\oprema.txt") { }
    
    public override Predmet KreirajEntitet(string[] podaciEntiteta)
    {
        Predmet p = new Predmet(podaciEntiteta[0]);
        p.Naziv = podaciEntiteta[1];
        p.Tip = ParseStringToTip(podaciEntiteta[3]);
        p.Obrisano = ParseStringToBool(podaciEntiteta[2]);
        return p;
    }

    public override string KreirajTextZaUpis(Predmet entitet)
    {
        string oprema = entitet.Id + "#" + entitet.Naziv + "#" + entitet.Obrisano + "#";
        if (entitet.Tip == TipOpreme.staticka)
        {
            oprema = oprema + "0#";
        }
        else
        {
            oprema = oprema + "1#";
        }
        return oprema;
    }

    public TipOpreme ParseStringToTip(string tekst)
    {
        if (tekst.Equals("0"))
        {
            return TipOpreme.staticka;
        }
        else
        {
            return TipOpreme.dinamicka;
        }
    }

    public bool ParseStringToBool(string tekst)
    {
        if (tekst.Equals("False"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}