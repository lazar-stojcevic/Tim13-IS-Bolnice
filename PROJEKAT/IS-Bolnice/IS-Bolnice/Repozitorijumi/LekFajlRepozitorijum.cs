// File:    LekFajlRepozitorijum.cs
// Author:  Zola
// Created: Sunday, April 11, 2021 7:46:32 PM
// Purpose: Definition of Class LekFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;

public class LekFajlRepozitorijum: GenerickiFajlRepozitorijum<Lek>, ILekRepozitorijum
{
    public LekFajlRepozitorijum() : base(@"..\..\Datoteke\lekovi.txt")
    {

    }
    
    public override Lek KreirajEntitet(string[] podaciEntiteta)
    {
        Lek lek = new Lek();

        lek.Id = podaciEntiteta[0];
        lek.Ime = podaciEntiteta[1];
        lek.Opis = podaciEntiteta[2];

        PostaviDaLiJePotrebanRecept(podaciEntiteta, lek);

        KreirajAlergene(podaciEntiteta, lek);

        KreirajZamenskeLekove(podaciEntiteta, lek);

        return lek;
    }

    public override string KreirajTextZaUpis(Lek lek)
    {
        string novaLinija = lek.Id + "#" + lek.Ime + "#" + lek.Opis + "#";
        if (lek.PotrebanRecept)
        {
            novaLinija += "1#";
        }
        else novaLinija += "0#";


        if (lek.Alergeni.Count != 0)
        {
            foreach (Sastojak sastojak in lek.Alergeni)
            {
                novaLinija += sastojak.Ime + ",";
            }
        }
        else
        {
            novaLinija += "nema,";
        }
        novaLinija = novaLinija.Remove(novaLinija.Length - 1);

        novaLinija = novaLinija + "#";
        if (lek.ZamenskiLekovi.Count != 0)
        {
            foreach (Lek zamenskiLek in lek.ZamenskiLekovi)
            {
                novaLinija = novaLinija + zamenskiLek.Id + "/";
            }
            novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        }

        return novaLinija;
    }

    private static void PostaviDaLiJePotrebanRecept(string[] delovi, Lek p)
    {
        if (delovi[3].Equals("1"))
        {
            p.PotrebanRecept = true;
        }
        else
        {
            p.PotrebanRecept = false;
        }
    }

    private static void KreirajAlergene(string[] delovi, Lek p)
    {
        string alergeniSvi = delovi[4];
        if (!alergeniSvi.Equals(""))
        {
            string[] alergen = alergeniSvi.Split(',');
            foreach (string a in alergen)
            {
                if (!a.Equals(""))
                {
                    Sastojak s = new Sastojak(a);
                    p.Alergeni.Add(s);
                }
            }
        }
    }

    private static void KreirajZamenskeLekove(string[] delovi, Lek p)
    {
        string zamenskiLekoviSvi = delovi[5];
        if (!zamenskiLekoviSvi.Equals(""))
        {
            string[] zameskiLek = zamenskiLekoviSvi.Split('/');
            foreach (string deo in zameskiLek)
            {
                if (deo.Equals("")) continue;
                Lek lek = new Lek(deo);
                p.ZamenskiLekovi.Add(lek);
            }
        }
    }
    

}