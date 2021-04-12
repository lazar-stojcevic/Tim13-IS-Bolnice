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
        List<Operacija> ret = new List<Operacija>();
        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        BazaBolnica bazaBolnica = new BazaBolnica();
        BazaLekara bazaLekara = new BazaLekara();
        List<Bolnica> bolnice = bazaBolnica.SveBolnice();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();
        List<Lekar> lekari = bazaLekara.SviLekari();
        if (File.Exists(@"..\..\Datoteke\operacije.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\operacije.txt");
            foreach (string line in lines)
            {
                Operacija o = new Operacija();
                string[] delovi = line.Split('#');
                Console.WriteLine(delovi[0]);
                o.VremePocetaOperacije = DateTime.Parse(delovi[0]);
                o.VremeKrajaOperacije = DateTime.Parse(delovi[1]);
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
                foreach (Lekar lekar in lekari)
                {
                    if (delovi[3].Equals(lekar.Jmbg))
                    {
                        o.Lekar = lekar;
                        break;
                    }
                }
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
        string path = @"..\..\Datoteke\operacije.txt";

        List<Operacija> operacije = SveSledeceOperacije();
        List<string> lines = new List<string>();

        foreach (Operacija o in operacije)
        {
            if (!(o.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) && o.Soba.Id.Equals(operacija.Soba.Id) && o.VremePocetaOperacije.Equals(operacija.VremePocetaOperacije)))
            {
                string zakazivanje = o.VremePocetaOperacije.ToString() + "#" + o.VremeKrajaOperacije.ToString() + "#" +
                    o.Pacijent.Jmbg + "#" + o.Lekar.Jmbg + "#" + o.Soba.Id;
                lines.Add(zakazivanje);
            }

        }

        File.WriteAllLines(path, lines);
    }
   
   public void IzmeniOperaciju(Operacija novaOperacija, Operacija staraOperacija)
   {
        OtkaziOperaciju(staraOperacija);
        ZakaziOperaciju(novaOperacija);
   }
   
   public List<Operacija> OperacijeOdredjenogPacijenta(Pacijent pacijent)
   {
        List<Operacija> operacije = new List<Operacija>();
        List<Operacija> sveOperacije = SveSledeceOperacije();

        foreach (Operacija o in sveOperacije)
        {
            if (o.Pacijent.Jmbg.Equals(pacijent.Jmbg))
            {
                operacije.Add(o);
            }
        }

        return operacije;
    }

    public List<Operacija> SveSledeceOperacijeZaLekara(string sifra)
    {
        List<Operacija> ret = new List<Operacija>();
        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        BazaBolnica bazaBolnica = new BazaBolnica();
        List<Bolnica> bolnice = bazaBolnica.SveBolnice();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();
        if (File.Exists(@"..\..\Datoteke\operacije.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\operacije.txt");
            foreach (string line in lines)
            {
                Operacija o = new Operacija();
                string[] delovi = line.Split('#');
                Console.WriteLine(delovi[3] + "                           " + sifra);
                if (delovi[3].Equals(sifra))
                {
                    Console.WriteLine(delovi[0]);
                    o.VremePocetaOperacije = DateTime.Parse(delovi[0]);
                    o.VremeKrajaOperacije = DateTime.Parse(delovi[1]);
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
                    Console.WriteLine("USPESNO JE NASAO JEDNU OPERACIJUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
                }
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;

    }

    public string fileLocation;

}