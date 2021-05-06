using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class BazaPregleda
{
    private static string fileLocation = @"..\..\Datoteke\pregledi.txt";
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    private BazaIzmena bazaIzmena = new BazaIzmena();
    private BazaLekara bazaLekara = new BazaLekara();
    private BazaOperacija bazaOperacija = new BazaOperacija();

    private static int BROJ_DANA_ZA_ZAKAZIVANJE_PREGLEDA = 4;

    //OVA METODA VRACA LISTU SLOBODNIH PREGLEDA KOJI MOGU DA SE ZAKAZU KOD PROSLEDJENOG LEKARA
    public List<Pregled> PonudjeniSlobodniPreglediLekara(string jmbgLekara)
    {
        Lekar lekar = PronadjiLekara(jmbgLekara);

        List<Pregled> validni = new List<Pregled>();

        foreach (Pregled predlozeniPregled in ListaSlobodnihTerminaPregledaTokomRadnogVremenaLekara(lekar))
        {
            if (FiltriranjeSlobodnihTerminaLekara(jmbgLekara, predlozeniPregled))
            {
                validni.Add(predlozeniPregled);
            }
        }

        return validni;
    }

    public List<Pregled> ListaSlobodnihTerminaPregledaTokomRadnogVremenaLekara(Lekar lekar)
    {
        List<Pregled> slobodni = new List<Pregled>();

        DateTime sutra = DateTime.Now.AddDays(1);

        for (int i = 0; i < BROJ_DANA_ZA_ZAKAZIVANJE_PREGLEDA; i++)
        {
            System.DateTime pocetakIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.PocetakRadnogVremena.Hour, lekar.PocetakRadnogVremena.Minute, 0, 0);
            System.DateTime krajIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.KrajRadnogVremena.Hour, lekar.KrajRadnogVremena.Minute, 0, 0);

            pocetakIntervala = pocetakIntervala.AddDays(i);
            krajIntervala = krajIntervala.AddDays(i);

            krajIntervala = krajIntervala.AddMinutes(-30);

            while (pocetakIntervala <= krajIntervala)
            {
                Pregled pregled = new Pregled();
                pregled.Lekar = lekar;
                pregled.VremePocetkaPregleda = pocetakIntervala;
                pregled.VremeKrajaPregleda = pregled.VremePocetkaPregleda.AddMinutes(30);
                slobodni.Add(pregled);

                pocetakIntervala = pocetakIntervala.AddMinutes(10);
            }
        }
        return slobodni;
    }

    public bool PreklapanjeTerminaPregleda(Pregled predlozeniPregled, Pregled zakazaniPregled)
    {
        if (predlozeniPregled.VremePocetkaPregleda == zakazaniPregled.VremePocetkaPregleda)
        {
            return false;
        }
        if (predlozeniPregled.VremePocetkaPregleda > zakazaniPregled.VremePocetkaPregleda && predlozeniPregled.VremePocetkaPregleda < zakazaniPregled.VremeKrajaPregleda)
        {
            return false;
        }
        if (predlozeniPregled.VremeKrajaPregleda > zakazaniPregled.VremePocetkaPregleda && predlozeniPregled.VremeKrajaPregleda < zakazaniPregled.VremeKrajaPregleda)
        {
            return false;
        }
        return true;
    }

    public bool PreklapanjeTerminaOperacija(Pregled predlozeniPregled, Operacija zakazanaOperacija)
    {
        if (predlozeniPregled.VremePocetkaPregleda == zakazanaOperacija.VremePocetkaOperacije)
        {
            return false;
        }
        if (predlozeniPregled.VremePocetkaPregleda > zakazanaOperacija.VremePocetkaOperacije && predlozeniPregled.VremePocetkaPregleda < zakazanaOperacija.VremeKrajaOperacije)
        {
            return false;
        }
        if (predlozeniPregled.VremeKrajaPregleda > zakazanaOperacija.VremePocetkaOperacije && predlozeniPregled.VremeKrajaPregleda < zakazanaOperacija.VremeKrajaOperacije)
        {
            return false;
        }
        return true;
    }

    public bool FiltriranjeSlobodnihTerminaLekara(string jmbgLekara, Pregled predlozeniPregled)
    {
        bool isValid = true;

        foreach (Pregled zakazaniPregled in SviBuduciPreglediKojeLekarIma(jmbgLekara))
        {
            isValid = PreklapanjeTerminaPregleda(predlozeniPregled, zakazaniPregled);
        }

        foreach (Operacija zakazanaOperacija in bazaOperacija.SveSledeceOperacijeDatogLekara(jmbgLekara))
        {
            isValid = PreklapanjeTerminaOperacija(predlozeniPregled, zakazanaOperacija);
        }

        return isValid;
    }

    //PREDLAZE TERMINE ZA IZMENU, SREDITI PROSLEDJENOG LEKARA DA IMA TACNO RADNO VREME
    public List<Pregled> SlobodniTerminiZaIzmenu(Lekar l, DateTime datum)
    {
        List<Pregled> validni = new List<Pregled>();
        List<Pregled> slobodni = new List<Pregled>();

        List<Lekar> lekari = bazaLekara.LekariOpstePrakse();
        Lekar lekar = new Lekar();
        foreach (Lekar ll in lekari)
        {
            if (l.Jmbg == ll.Jmbg)
            {
                lekar = ll;
            }
        }

        System.DateTime pocetakIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.PocetakRadnogVremena.Hour, lekar.PocetakRadnogVremena.Minute, 0, 0);
        System.DateTime krajIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.KrajRadnogVremena.Hour, lekar.KrajRadnogVremena.Minute, 0, 0);
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
            foreach (Pregled zakazani in SviBuduciPreglediKojeLekarIma(l.Jmbg))
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
        foreach (Pregled zakazaniPregled in SviBuduciPreglediKojePacijentIma(pregled.Pacijent.Jmbg))
        {
            if (zakazaniPregled.VremePocetkaPregleda > DateTime.Now)
            {
                if (pregled.VremePocetkaPregleda == zakazaniPregled.VremePocetkaPregleda)
                {
                    return true;
                }
                else if (pregled.VremePocetkaPregleda > zakazaniPregled.VremePocetkaPregleda && pregled.VremePocetkaPregleda < zakazaniPregled.VremeKrajaPregleda)
                {
                    return true;
                }
                else if (zakazaniPregled.VremePocetkaPregleda < pregled.VremeKrajaPregleda && pregled.VremeKrajaPregleda < zakazaniPregled.VremeKrajaPregleda)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool ZakazivanjePregledaUTerminu(Pregled pregled)
    {
        List<Pregled> sviPregledi = SviPregledi();
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
                List<Pregled> zakazaniPregledi = SviBuduciPreglediKojeLekarIma(l.Jmbg);
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

    public List<Pregled> SviBuduciPreglediKojePacijentIma(string jmbgPacijenta)
    {
        List<Pregled> pregledi = new List<Pregled>();

        foreach (Pregled pregled in SviPregledi())
        {
            if (pregled.Pacijent.Jmbg.Equals(jmbgPacijenta) && pregled.VremePocetkaPregleda > DateTime.Now)
            {
                pregledi.Add(pregled);
            }
        }

        pregledi.Sort((x, y) => x.VremePocetkaPregleda.CompareTo(y.VremePocetkaPregleda));

        return pregledi;
    }

    public List<Pregled> SviBuduciPreglediKojeLekarIma(string jmbgLekara)
    {
        List<Pregled> pregledi = new List<Pregled>();

        foreach (Pregled p in SviPregledi())
        {
            if (p.Lekar.Jmbg.Equals(jmbgLekara) && p.VremePocetkaPregleda > DateTime.Now)
            {
                pregledi.Add(p);
            }
        }

        return pregledi;
    }

    //OPTIMIZOVATI KOD
    public List<Pregled> SviPregledi()
    {
        BazaLekara bl = new BazaLekara();

        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();

        List<string> lines = new List<string>();
        List<Pregled> pregledi = new List<Pregled>();
        List<Lekar> lekari = bl.SviLekari();

        lines = File.ReadAllLines(fileLocation).ToList();

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
            p.VremePocetkaPregleda = DateTime.ParseExact(vremePocetka, vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            p.VremeKrajaPregleda = DateTime.ParseExact(vremeKraja, vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
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

    public void ZakaziPregled(Pregled noviPregled)
    {
        bool isValid = true;
        foreach (Pregled zakazani in SviPregledi())
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
            string zakazivanje = FormatPisanjaPregleda(noviPregled);

            List<string> pregledi = new List<string>();
            pregledi.Add(zakazivanje);
            File.AppendAllLines(fileLocation, pregledi);

            SaveChangeInBase(noviPregled.Pacijent.Jmbg);
        }
    }

    // refaktorisane
    public void OtkaziPregled(Pregled pregledZaBrisanje)
    {
        List<string> redoviZaUpisUDatoteku = new List<string>();

        foreach (Pregled pregled in SviPregledi())
        {
            if (pregled.Lekar.Jmbg != pregledZaBrisanje.Lekar.Jmbg || !pregled.VremePocetkaPregleda.Equals(pregledZaBrisanje.VremePocetkaPregleda))
            {
                string zakazivanje = FormatPisanjaPregleda(pregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
            }

        }
        File.WriteAllLines(fileLocation, redoviZaUpisUDatoteku);
    }

    public void IzmeniPregled(Pregled noviPregled, Pregled stariPregled)
    {
        List<string> redoviZaUpisUDatoteku = new List<string>();

        foreach (Pregled pregled in SviPregledi())
        {
            if (pregled.Lekar.Jmbg != stariPregled.Lekar.Jmbg || !pregled.VremePocetkaPregleda.Equals(stariPregled.VremePocetkaPregleda))
            {
                string zakazivanje = FormatPisanjaPregleda(pregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
            }
            else
            {
                string zakazivanje = FormatPisanjaPregleda(noviPregled);
                redoviZaUpisUDatoteku.Add(zakazivanje);
                SaveChangeInBase(noviPregled.Pacijent.Jmbg);
            }

        }
        File.WriteAllLines(fileLocation, redoviZaUpisUDatoteku);
    }

    public string FormatPisanjaPregleda(Pregled pregled)
    {
        return pregled.Pacijent.Jmbg + "#" + pregled.Lekar.Jmbg + "#" + pregled.VremePocetkaPregleda.ToString(vremenskiFormatPisanje)
            + "#" + pregled.VremeKrajaPregleda.ToString(vremenskiFormatPisanje) + "#" + "Broj ordinacije";
    }

    public Lekar PronadjiLekara(string jmbgLekara)
    {
        foreach (Lekar lekar in bazaLekara.SviLekari())
        {
            if (lekar.Jmbg.Equals(jmbgLekara))
            {
                return lekar;
            }
        }
        return null;
    }

    public void SaveChangeInBase(string jmbgPacijenta)
    {
        Change change = new Change();
        DateTime now = DateTime.Now;

        change.DateOfChange = now;
        change.JmbgOfPatient = jmbgPacijenta;

        bazaIzmena.SaveChange(change);
    }
}