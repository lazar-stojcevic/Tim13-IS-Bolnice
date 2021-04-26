using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class BazaPregleda
{
    public List<Pregled> PonudjeniSlobodniPreglediLekara(string jmbgLekara)
    {
        //Lekar
        Lekar lekar = new Lekar();
        BazaLekara bl = new BazaLekara();
        BazaOperacija bazaOperacija = new BazaOperacija();
        List<Lekar> lekari = bl.SviLekari();
        foreach (Lekar l in lekari)
        {
            if (l.Jmbg.Equals(jmbgLekara))
            {
                lekar = l;
                break;
            }
        }
        //Liste
        List<Pregled> validni = new List<Pregled>();
        List<Pregled> zauzeti = PreglediOdredjenogLekara(jmbgLekara);
        List<Operacija> zauzeteOperacije = bazaOperacija.SveSledeceOperacijeDatogLekara(jmbgLekara);
        List<Pregled> slobodni = new List<Pregled>();

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
                Pregled p = new Pregled();
                p.Lekar = lekar;
                p.VremePocetkaPregleda = pocetakIntervala;
                pocetakIntervala = pocetakIntervala.AddMinutes(10);
                p.VremeKrajaPregleda = p.VremePocetkaPregleda.AddMinutes(30);
                slobodni.Add(p);
            }
        }

        foreach (Pregled predlozeni in slobodni)
        {
            bool isValid = true;
            //Provera da li lekar ima zakazan pregled u nekom periodu
            foreach (Pregled zakazani in zauzeti)
            {
                if (predlozeni.VremePocetkaPregleda == zakazani.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
            }
            //Provera da li lekar ima zakazanu operaciju u nekom periodu
            foreach (Operacija zakazani in zauzeteOperacije)
            {
                if (predlozeni.VremePocetkaPregleda == zakazani.VremePocetaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaPregleda > zakazani.VremePocetaOperacije && predlozeni.VremePocetkaPregleda < zakazani.VremeKrajaOperacije)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaPregleda > zakazani.VremePocetaOperacije && predlozeni.VremeKrajaPregleda < zakazani.VremeKrajaOperacije)
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

    //PREDLAZE TERMINE ZA IZMENU, SREDITI PROSLEDJENOG LEKARA DA IMA TACNO RADNO VREME
    public List<Pregled> slobodniTerminiZaIzmenu(Lekar l, DateTime datum)
    {
        List<Pregled> validni = new List<Pregled>();
        List<Pregled> slobodni = new List<Pregled>();
        List<Pregled> zauzeti = new List<Pregled>();

        BazaLekara bl = new BazaLekara();
        List<Lekar> lekari = bl.LekariOpstePrakse();
        Lekar lekar = new Lekar();
        foreach (Lekar ll in lekari)
        {
            if (l.Jmbg == ll.Jmbg)
            {
                lekar = ll;
            }
        }

        Console.WriteLine("Lekar: " + lekar.Ime);

        zauzeti = PreglediOdredjenogLekara(l.Jmbg);

        System.DateTime pocetakIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.PocetakRadnogVremena.Hour, lekar.PocetakRadnogVremena.Minute, 0, 0);
        Console.WriteLine("POCETAK " + pocetakIntervala);
        System.DateTime krajIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.KrajRadnogVremena.Hour, lekar.KrajRadnogVremena.Minute, 0, 0);
        Console.WriteLine("KRAJ " + krajIntervala);
        krajIntervala = krajIntervala.AddMinutes(-30);

        while (pocetakIntervala <= krajIntervala)
        {
            Pregled p = new Pregled();
            p.Lekar = lekar;
            p.VremePocetkaPregleda = pocetakIntervala;
            pocetakIntervala = pocetakIntervala.AddMinutes(10);
            p.VremeKrajaPregleda = p.VremePocetkaPregleda.AddMinutes(30);
            slobodni.Add(p);
        }

        foreach (Pregled predlozeni in slobodni)
        {
            bool isValid = true;
            foreach (Pregled zakazani in zauzeti)
            {
                if (predlozeni.VremePocetkaPregleda == zakazani.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
                if (predlozeni.VremeKrajaPregleda > zakazani.VremePocetkaPregleda && predlozeni.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
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

    //PROVERA DA LI PACIJENT VEC IMA ZAKAZANI PREGLED U TOM TERMINU
    public bool PacijentImaZakazanPregled(Pregled pregled)
    {
        List<Pregled> pregledi = PreglediOdredjenogPacijenta(pregled.Pacijent.Jmbg);

        foreach (Pregled zakazani in pregledi)
        {
            if (zakazani.VremePocetkaPregleda > DateTime.Now)
            {
                // |=* -> ||=**
                if (pregled.VremePocetkaPregleda == zakazani.VremePocetkaPregleda)
                {
                    return true;
                }
                // |*|*
                else if (pregled.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && pregled.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    return true;
                }
                // *|*|
                else if (zakazani.VremePocetkaPregleda < pregled.VremeKrajaPregleda && pregled.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool ZakazivanjePregledaUTerminu(Pregled pregled)
    {
        List<Pregled> sviPregledi = SviSledeciPregledi();
        BazaLekara bl = new BazaLekara();
        List<Lekar> lekari = bl.LekariOpstePrakse();

        //PROLAZIM KROZ SVE LEKARE OPSTE PRAKSE
        foreach (Lekar l in lekari)
        {
            //FORMIRANJE RADNOG VREMENA LEKARA ZA DAN PROSLEDJENOG PREGLEDA
            System.DateTime pocetakRadnogVremena = new System.DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month, pregled.VremePocetkaPregleda.Day,
                l.PocetakRadnogVremena.Hour, l.PocetakRadnogVremena.Minute, 0, 0);
            System.DateTime krajRadnogVremena = new System.DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month, pregled.VremePocetkaPregleda.Day,
                l.KrajRadnogVremena.Hour, l.KrajRadnogVremena.Minute, 0, 0);

            //AKO PREGLED UPADA U RADNO VREME POREDI SE SA OSTALIM PREGLEDIMA ZBOG PREKLAPANJA
            if (pregled.VremePocetkaPregleda >= pocetakRadnogVremena && pregled.VremeKrajaPregleda <= krajRadnogVremena)
            {
                List<Pregled> zakazaniPregledi = PreglediOdredjenogLekara(l.Jmbg);
                bool isValid = true;
                foreach (Pregled zakazani in zakazaniPregledi)
                {
                    // |=* -> ||=**
                    if (pregled.VremePocetkaPregleda == zakazani.VremePocetkaPregleda)
                    {
                        isValid = false;
                        break;
                    }
                    // |*|*
                    else if (pregled.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && pregled.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                    {
                        isValid = false;
                        break;
                    }
                    // *|*|
                    else if (zakazani.VremePocetkaPregleda < pregled.VremeKrajaPregleda && pregled.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                    {
                        isValid = false;
                        break;
                    }

                }
                //AKO LEKAR IMA SLOBODAN TERMIN, NJEMU SE DODELJUJE PROSLEDJENI PREGLED
                if (isValid)
                {
                    pregled.Lekar = l;
                    ZakaziPregled(pregled);
                    return true;
                }
            }

        }
        return false;
    }

    public List<Pregled> PreglediOdredjenogPacijenta(string jmbg)
    {
        List<Pregled> pregledi = new List<Pregled>();
        List<Pregled> sviPregledi = SviSledeciPregledi();

        foreach (Pregled p in sviPregledi)
        {
            if (p.Pacijent.Jmbg.Equals(jmbg) && p.VremePocetkaPregleda > DateTime.Now)
            {
                pregledi.Add(p);
            }
        }
        //sortiranje
        pregledi.Sort((x, y) => x.VremePocetkaPregleda.CompareTo(y.VremePocetkaPregleda));

        return pregledi;
    }

    public List<Pregled> PreglediOdredjenogLekara(string jmbg)
    {
        List<Pregled> pregledi = new List<Pregled>();
        List<Pregled> sviPregledi = SviSledeciPregledi();

        foreach (Pregled p in sviPregledi)
        {
            if (p.Lekar.Jmbg.Equals(jmbg) && p.VremePocetkaPregleda > DateTime.Now)
            {
                pregledi.Add(p);
            }
        }

        return pregledi;
    }

    //OPTIMIZOVATI KOD
    public List<Pregled> SviSledeciPregledi()
    {
        string path = @"..\..\Datoteke\pregledi.txt";

        BazaLekara bl = new BazaLekara();

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
            pac.Jmbg = jmbgPacijenta;
            p.Pacijent = pac;
            p.VremePocetkaPregleda = DateTime.Parse(vremePocetka);
            p.VremeKrajaPregleda = DateTime.Parse(vremeKraja);
            //dodati broj sobe ovde
            foreach (Lekar l in lekari)
            {
                if (l.Jmbg == jmbgLekara)
                {
                    p.Lekar = l;
                }
            }

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
    //METODA PROMENJENA U BOOL: BI TREBALO
    public void ZakaziPregled(Pregled noviPregled)
    {
        // PROVERA DA LI JE U MEDJUVREMENU NEKO ZAUZEO TERMIN KOD LEKARA
        List<Pregled> sviPregledi = SviSledeciPregledi();
        bool isValid = true;
        foreach (Pregled zakazani in sviPregledi)
        {
            if (noviPregled.Lekar.Jmbg == zakazani.Lekar.Jmbg)
            {
                if (zakazani.VremePocetkaPregleda == noviPregled.VremePocetkaPregleda)
                {
                    isValid = false;
                    break;
                }
                else if (noviPregled.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && noviPregled.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
                else if (zakazani.VremePocetkaPregleda < noviPregled.VremeKrajaPregleda && noviPregled.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                {
                    isValid = false;
                    break;
                }
            }
        }
        if (!isValid)
        {
            Console.WriteLine("Neko je zauzeo termin u medjuvremenu");
        }
        else
        {
            string path = @"..\..\Datoteke\pregledi.txt";
            string jmbgPacijenta = noviPregled.Pacijent.Jmbg;
            string jmbgLekara = noviPregled.Lekar.Jmbg;
            DateTime vremePocetka = noviPregled.VremePocetkaPregleda;
            DateTime vremeKraja = noviPregled.VremeKrajaPregleda;

            //dodati broj sobe lekaru
            //promenjen upis
            string zakazivanje = jmbgPacijenta + "#" + jmbgLekara + "#" + vremePocetka.ToString() + "#" + vremeKraja.ToString()
                + "#" + "Broj ordinacije";

            List<string> pregledi = new List<string>();
            pregledi.Add(zakazivanje);
            File.AppendAllLines(path, pregledi);
        }

    }

    public void OtkaziPregled(Pregled pregled)
    {
        // dodati metodu za izlistavanje svih mogucih pregleda
        string path = @"..\..\Datoteke\pregledi.txt";

        List<Pregled> pregedi = SviSledeciPregledi();
        List<string> lines = new List<string>();

        foreach (Pregled p in pregedi)
        {
            if (p.Lekar.Jmbg != pregled.Lekar.Jmbg || !p.VremePocetkaPregleda.Equals(pregled.VremePocetkaPregleda))
            {
                // menjanje upisa
                string zakazivanje = p.Pacijent.Jmbg + "#" + p.Lekar.Jmbg + "#" + p.VremePocetkaPregleda.ToString() + "#" + p.VremeKrajaPregleda.ToString() + "#" + "brojSobe";
                lines.Add(zakazivanje);
            }

        }

        File.WriteAllLines(path, lines);
    }

    //DODATI PROVERU DA LI JE U MEDJUVREMENU PROSLEDJENI PREGLED ZAUZET
    public void IzmeniPregled(Pregled noviPregled, Pregled stariPregled)
    {
        string path = @"..\..\Datoteke\pregledi.txt";

        List<Pregled> pregedi = SviSledeciPregledi();
        List<string> lines = new List<string>();

        foreach (Pregled p in pregedi)
        {
            if (p.Lekar.Jmbg != stariPregled.Lekar.Jmbg || !p.VremePocetkaPregleda.Equals(stariPregled.VremePocetkaPregleda))
            {
                string zakazivanje = p.Pacijent.Jmbg + "#" + p.Lekar.Jmbg + "#" + p.VremePocetkaPregleda.ToString() + "#" + p.VremeKrajaPregleda.ToString() + "#" + "brojSobe";
                lines.Add(zakazivanje);
            }
            else
            {
                // upisivanje pregleda sa izmenjenim terminom
                string zakazivanje = noviPregled.Pacijent.Jmbg + "#" + noviPregled.Lekar.Jmbg + "#" + noviPregled.VremePocetkaPregleda.ToString() + "#" + noviPregled.VremeKrajaPregleda.ToString() + "#" + "brojSobe";
                lines.Add(zakazivanje);
            }

        }

        File.WriteAllLines(path, lines);
    }

    public string fileLocation;

}