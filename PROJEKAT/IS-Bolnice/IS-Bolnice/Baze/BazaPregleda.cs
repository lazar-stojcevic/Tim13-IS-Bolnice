// File:    BazaPregleda.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class BazaPregleda

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BazaPregleda
{
    public List<Pregled> SviSledeciPregledi()
    {
        string path = @"..\..\Datoteke\pregledi.txt";

        BazaLekara bl = new BazaLekara();
        //DODATO NA BOJANOV KOD
        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();

        List<string> lines = new List<string>();
        List<Pregled> pregledi = new List<Pregled>();
        List<Lekar> lekari = bl.SviLekari();

        lines = File.ReadAllLines(path).ToList();

        foreach (string line in lines)
        {
            string[] items = line.Split('#');

            Pregled p = new Pregled();
            Pacijent pac = new Pacijent();
            string jmbgPacijenta = items[0];
            string jmbgLekara = items[1];
            string vremePocetka = items[2];
            string vremeKraja = items[3];
            //dodati broj sobe

            foreach (Lekar l in lekari)
            {
                if (l.Jmbg == jmbgLekara)
                {
                    p.Lekar = l;
                }
            }
            // dodati pacijenta
            pac.Jmbg = jmbgPacijenta;
            p.Pacijent = pac;
            p.VremePocetkaPregleda = DateTime.Parse(vremePocetka);
            p.VremeKrajaPregleda = DateTime.Parse(vremeKraja);
            //DODATO NA BOJANOV KOD
            foreach (Pacijent pacijent in pacijenti)
            {
                if (p.Pacijent.Jmbg.Equals(pacijent.Jmbg))
                {
                    p.Pacijent.Prezime = pacijent.Prezime;
                    p.Pacijent.Ime = pacijent.Ime;
                    break;
                }
            }
            pregledi.Add(p);
        }
        return pregledi;
    }

    public void ZakaziPregled(Pregled noviPregled)
    {
        string path = @"..\..\Datoteke\pregledi.txt";
        string jmbgPacijenta = noviPregled.Pacijent.Jmbg;
        string jmbgLekara = noviPregled.Lekar.Jmbg;
        DateTime vremePocetka = noviPregled.VremePocetkaPregleda;
        DateTime vremeKraja = noviPregled.VremeKrajaPregleda;

        //dodati broj sobe lekaru
        string zakazivanje = jmbgPacijenta + "#" + jmbgLekara + "#" + vremePocetka.ToString() + "#" + vremeKraja.ToString() + "#" + "Broj ordinacije";

        List<string> pregledi = new List<string>();
        pregledi.Add(zakazivanje);

        File.AppendAllLines(path, pregledi);
    }

    public void OtkaziPregled(Pregled pregled)
    {
        // dodati metodu za izlistavanje svih mogucih pregleda
        string path = @"..\..\Datoteke\pregledi.txt";

        List<Pregled> pregedi = SviSledeciPregledi();
        List<string> lines = new List<string>();

        Console.WriteLine(pregled.Lekar.Jmbg);

        foreach (Pregled p in pregedi)
        {
            if (p.Lekar.Jmbg != pregled.Lekar.Jmbg || !p.VremePocetkaPregleda.Equals(pregled.VremePocetkaPregleda))
            {
                string zakazivanje = p.Pacijent.Jmbg + "#" + p.Lekar.Jmbg + "#" + p.VremePocetkaPregleda.ToString() + "#" + p.VremeKrajaPregleda.ToString() + "#" + "brojSobe";
                lines.Add(zakazivanje);
            }

        }

        File.WriteAllLines(path, lines);
    }

    public void IzmeniPregled(Pregled pregled)
   {
      throw new NotImplementedException();
   }
   
   public List<Pregled> PreglediDatogPacijenta(Pacijent pacijent)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}