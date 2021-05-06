using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class BazaOperacija
{
    public string fileLocation;
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

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
        List<Pregled> zauzeti = bazaPregleda.SviBuduciPreglediKojeLekarIma(jmbgLekara);
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
                o.VremePocetkaOperacije = pocetakIntervala;
                pocetakIntervala = pocetakIntervala.AddMinutes(10);
                o.VremeKrajaOperacije = o.VremePocetkaOperacije.AddMinutes(45);
                slobodni.Add(o);
            }
        }

        foreach (Operacija predlozeni in slobodni)
        {
            bool isValid = true;
            //Provera da li lekar ima zakazan pregled u nekom periodu
            foreach (Pregled zakazani in zauzeti)
            {
                if (predlozeni.VremePocetkaOperacije == zakazani.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaOperacije > zakazani.VremePocetkaPregleda && predlozeni.VremePocetkaOperacije < zakazani.VremeKrajaPregleda)
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
                if (predlozeni.VremePocetkaOperacije == zakazani.VremePocetkaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaOperacije > zakazani.VremePocetkaOperacije && predlozeni.VremePocetkaOperacije < zakazani.VremeKrajaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaOperacije > zakazani.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije < zakazani.VremeKrajaOperacije)
                {
                    isValid = false;
                    break;
                }
            }
            foreach (Operacija operacija in SveSledeceOperacije())
            {
                if (operacija.Soba.Id.Equals(idSale) 
                    && ((predlozeni.VremePocetkaOperacije > operacija.VremePocetkaOperacije && predlozeni.VremePocetkaOperacije < operacija.VremeKrajaOperacije) 
                    || (predlozeni.VremeKrajaOperacije > operacija.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije < operacija.VremeKrajaOperacije)
                    || (predlozeni.VremePocetkaOperacije == operacija.VremePocetkaOperacije)))
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
                o.VremePocetkaOperacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                o.VremeKrajaOperacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
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

                if (delovi[5].Equals("True"))
                {
                    o.Hitna = true;
                }
                else o.Hitna = false;

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
        string linija = novaOperacija.VremePocetkaOperacije.ToString(vremenskiFormatPisanje) + "#" + novaOperacija.VremeKrajaOperacije.ToString(vremenskiFormatPisanje) + "#" +
            novaOperacija.Pacijent.Jmbg + "#" + novaOperacija.Lekar.Jmbg + "#" + novaOperacija.Soba.Id + "#" + novaOperacija.Hitna + System.Environment.NewLine;
        File.AppendAllText(@"..\..\Datoteke\operacije.txt", linija);
    }

    public void OtkaziOperaciju(Operacija operacija)
   {
        string path = @"..\..\Datoteke\operacije.txt";

        List<Operacija> operacije = SveSledeceOperacije();
        List<string> lines = new List<string>();

        foreach (Operacija o in operacije)
        {
            if (!(o.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) && o.Soba.Id.Equals(operacija.Soba.Id) && o.VremePocetkaOperacije.Equals(operacija.VremePocetkaOperacije)))
            {
                string zakazivanje = o.VremePocetkaOperacije.ToString(vremenskiFormatPisanje) + "#" + o.VremeKrajaOperacije.ToString(vremenskiFormatPisanje) + "#" +
                    o.Pacijent.Jmbg + "#" + o.Lekar.Jmbg + "#" + o.Soba.Id + "#" + o.Hitna;
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
                    o.VremePocetkaOperacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    o.VremeKrajaOperacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
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

                    if (delovi[5].Equals("True"))
                    {
                        o.Hitna = true;
                    }
                    else o.Hitna = false;

                    ret.Add(o);
                }
            }
        }
        return ret;

    }
}