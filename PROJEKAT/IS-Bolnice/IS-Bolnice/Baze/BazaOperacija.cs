using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class BazaOperacija
{
    public List<Operacija> SveSledeceOperacijeDatogLekara(string jmbgLekara)
    {
        List<Operacija> povratnaVrednost = new List<Operacija>();
        List<Operacija> sveNaredneOperacijeSvihLekara = SveSledeceOperacije();
        foreach (Operacija iterOperacija in sveNaredneOperacijeSvihLekara)
        {
            if (iterOperacija.Lekar.Jmbg.Equals(jmbgLekara)){
                povratnaVrednost.Add(iterOperacija);
            }
        }
        return povratnaVrednost;
    }

    public List<Operacija> PonudjeniSlobodniTerminiLekara(string jmbgLekara, string idSale)
    {
        //Baze
        BazaPregleda  bazaPregleda = new BazaPregleda();
        BazaLekara bazaLekara = new BazaLekara();
        BazaBolnica bazaBolnica = new BazaBolnica();
        //Lekar
        Lekar lekar = new Lekar();
        List<Lekar> lekari = bazaLekara.SviLekari();
        foreach (Lekar l in lekari)
        {
            if (l.Jmbg.Equals(jmbgLekara))
            {
                lekar = l;
                break;
            }
        }
        //Liste
        List<Operacija> validni = new List<Operacija>();
        List<Pregled> zauzeti = bazaPregleda.PreglediOdredjenogLekara(jmbgLekara);
        List<Operacija> zauzeteOperacije = SveSledeceOperacijeDatogLekara(jmbgLekara);
        //OVO JE UZASNO NEOPTIMALNO ALI JE BITNO DA PRORADI PRVO
        List<Operacija> sveOperacije = SveSledeceOperacije();
        List<Operacija> slobodni = new List<Operacija>();

        DateTime sutra = DateTime.Now.AddDays(1);
        for (int i = 0; i < 2; i++)
        {
            System.DateTime pocetakIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.PocetakRadnogVremena.Hour, lekar.PocetakRadnogVremena.Minute, 0, 0);
            System.DateTime krajIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.KrajRadnogVremena.Hour, lekar.KrajRadnogVremena.Minute, 0, 0);
            pocetakIntervala = pocetakIntervala.AddDays(i);
            krajIntervala = krajIntervala.AddDays(i);
            krajIntervala = krajIntervala.AddMinutes(-30);

            while (pocetakIntervala <= krajIntervala)
            {
                Operacija o = new Operacija();
                o.Lekar = lekar;
                o.VremePocetaOperacije = pocetakIntervala;
                pocetakIntervala = pocetakIntervala.AddMinutes(10);
                o.VremeKrajaOperacije = o.VremePocetaOperacije.AddMinutes(45);
                slobodni.Add(o);
            }
        }

        foreach (Operacija predlozeni in slobodni)
        {
            bool isValid = true;
            //Provera da li lekar ima zakazan pregled u nekom periodu
            foreach (Pregled zakazani in zauzeti)
            {
                if (predlozeni.VremePocetaOperacije == zakazani.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetaOperacije > zakazani.VremePocetkaPregleda && predlozeni.VremePocetaOperacije < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaOperacije > zakazani.VremePocetkaPregleda && predlozeni.VremeKrajaOperacije < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
            }
            //Provera da li lekar ima zakazanu operaciju u nekom periodu
            foreach (Operacija zakazani in zauzeteOperacije)
            {
                if (predlozeni.VremePocetaOperacije == zakazani.VremePocetaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetaOperacije > zakazani.VremePocetaOperacije && predlozeni.VremePocetaOperacije < zakazani.VremeKrajaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaOperacije > zakazani.VremePocetaOperacije && predlozeni.VremeKrajaOperacije < zakazani.VremeKrajaOperacije)
                {
                    isValid = false;
                    break;
                }
            }
            foreach (Operacija operacija in SveSledeceOperacije())
            {
                if (operacija.Soba.Id.Equals(idSale) 
                    && ((predlozeni.VremePocetaOperacije > operacija.VremePocetaOperacije && predlozeni.VremePocetaOperacije < operacija.VremeKrajaOperacije) 
                    || (predlozeni.VremeKrajaOperacije > operacija.VremePocetaOperacije && predlozeni.VremeKrajaOperacije < operacija.VremeKrajaOperacije)
                    || (predlozeni.VremePocetaOperacije == operacija.VremePocetaOperacije)))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                validni.Add(predlozeni);
            }
        }

        return validni;
    }

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
                }
            }
        }
        return ret;

    }

    public string fileLocation;

}