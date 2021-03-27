// File:    BazaPacijenata.cs
// Author:  Zola
// Created: Monday, March 22, 2021 7:57:46 PM
// Purpose: Definition of Class BazaPacijenata

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BazaPacijenata
{
    public static string fileLocation = @"..\..\Datoteke\pacijenti.txt";

    public List<Pacijent> SviPacijenti()
    {
        List<string> linije = new List<string>();
        linije = File.ReadAllLines(fileLocation).ToList();

        return NapraviPacijente(linije);
    }
   
    public void KreirajPacijenta(Pacijent pacijent)
    {
        List<string> pLista = new List<string>();
        pLista.Add(PacijentToString(pacijent));
        File.AppendAllLines(fileLocation, pLista);
    }
   
    public void ObrisiPacijenta(Pacijent pacijent)
    {
        List<string> linije = new List<string>();
        linije = File.ReadAllLines(fileLocation).ToList();

        List<Pacijent> pacijenti = new List<Pacijent>();
        pacijenti = NapraviPacijente(linije);

        List<string> pacijentiString = new List<string>();

        foreach (Pacijent p in pacijenti)
        {
            if (pacijent.Jmbg.Equals(p.Jmbg))
            {
                // ne dodavati u listu
            }
            else
            {
                pacijentiString.Add(PacijentToString(p));
            }
        }

        File.WriteAllLines(fileLocation, pacijentiString);
    }
   
    public void IzmeniPacijenta(Pacijent pacijentIzmenjen, Pacijent pacijentPocetni)
     {
        List<string> linije = new List<string>();
        linije = File.ReadAllLines(fileLocation).ToList();

        List<Pacijent> pacijenti = new List<Pacijent>();
        pacijenti = NapraviPacijente(linije);

        List<string> pacijentiString = new List<string>();

        foreach(Pacijent p in pacijenti)
        {
            if (pacijentPocetni.Jmbg.Equals(p.Jmbg))
            {
                pacijentiString.Add(PacijentToString(pacijentIzmenjen));
            }
            else
            {
                pacijentiString.Add(PacijentToString(p));
            }
        }

        File.WriteAllLines(fileLocation, pacijentiString);
     }

    private string PacijentToString(Pacijent pacijent)
    {
        string p = pacijent.Jmbg + "#" + pacijent.KorisnickoIme + "#" + pacijent.Sifra + "#" + pacijent.Ime + "#" +
            pacijent.Prezime + "#" + pacijent.BrojTelefona + "#" + pacijent.EMail + "#" + pacijent.Adresa + "#" + pacijent.Pol.ToString();

        return p;
    }

    private List<Pacijent> NapraviPacijente(List<string> linije)
    {
        List<Pacijent> pacijenti = new List<Pacijent>();

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split('#');

            Pacijent p = new Pacijent();
            p.Jmbg = delovi[0];
            p.KorisnickoIme = delovi[1];
            p.Sifra = delovi[2];
            p.Ime = delovi[3];
            p.Prezime = delovi[4];
            p.BrojTelefona = delovi[5];
            p.EMail = delovi[6];
            p.Adresa = delovi[7];
            if (delovi[8].Equals("muski"))
            {
                p.Pol = Pol.muski;
            }
            else if (delovi[8].Equals("zenski"))
            {
                p.Pol = Pol.zenski;
            }
            else
            {
                p.Pol = Pol.drugo;
            }

            pacijenti.Add(p);
        }
        return pacijenti;
    }

}