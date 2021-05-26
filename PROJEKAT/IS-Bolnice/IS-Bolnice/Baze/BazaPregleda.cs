using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

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

    public List<Pregled> SviPreglediUOdabranojSobi(string idSobe)
    {
        List<Pregled> preglediUSobi = new List<Pregled>();
        foreach (Pregled pregled in SviBuduciPregledi())
        {
            if (pregled.Lekar.Ordinacija.Id.Equals(idSobe))
            {
                preglediUSobi.Add(pregled);
            }
        }
        return preglediUSobi;
    }

    //OVA METODA VRACA LISTU SLOBODNIH PREGLEDA KOJI MOGU DA SE ZAKAZU KOD PROSLEDJENOG LEKARA
    public List<Pregled> PonudjeniSlobodniPreglediLekara(string jmbgLekara)
    {
        Lekar lekar = PronadjiLekara(jmbgLekara);

        List<Pregled> validni = new List<Pregled>();

        foreach (Pregled predlozeniPregled in ListaSlobodnihTerminaPregledaTokomRadnogVremenaLekara(lekar))
        {
            if (!TerminSePreklapaKodLekara(jmbgLekara, predlozeniPregled))
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
            System.DateTime pocetakIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
            System.DateTime krajIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);

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
        if ((predlozeniPregled.VremePocetkaPregleda <= zakazaniPregled.VremePocetkaPregleda) &&
            (predlozeniPregled.VremeKrajaPregleda >= zakazaniPregled.VremePocetkaPregleda))
        {
            return true;
        }

        if ((predlozeniPregled.VremePocetkaPregleda <= zakazaniPregled.VremePocetkaPregleda) &&
            (zakazaniPregled.VremeKrajaPregleda <= predlozeniPregled.VremeKrajaPregleda))
        {
            return true;
        }

        if ((zakazaniPregled.VremePocetkaPregleda <= predlozeniPregled.VremePocetkaPregleda) &&
            (predlozeniPregled.VremeKrajaPregleda) <= zakazaniPregled.VremeKrajaPregleda)
        {
            return true;
        }

        if ((zakazaniPregled.VremePocetkaPregleda <= predlozeniPregled.VremePocetkaPregleda) &&
            (zakazaniPregled.VremeKrajaPregleda >= predlozeniPregled.VremePocetkaPregleda))
        {
            return true;
        }

        return false;
    }

    public bool PreklapanjeTerminaPregledaSaOperacijom(Pregled predlozeniPregled, Operacija zakazanaOperacija)
    {
        if ((predlozeniPregled.VremePocetkaPregleda <= zakazanaOperacija.VremePocetkaOperacije) &&
            (predlozeniPregled.VremeKrajaPregleda >= zakazanaOperacija.VremePocetkaOperacije))
        {
            return true;
        }

        if ((predlozeniPregled.VremePocetkaPregleda <= zakazanaOperacija.VremePocetkaOperacije) &&
            (zakazanaOperacija.VremeKrajaOperacije <= predlozeniPregled.VremeKrajaPregleda))
        {
            return true;
        }

        if ((zakazanaOperacija.VremePocetkaOperacije <= predlozeniPregled.VremePocetkaPregleda) &&
            (predlozeniPregled.VremeKrajaPregleda) <= zakazanaOperacija.VremeKrajaOperacije)
        {
            return true;
        }

        if ((zakazanaOperacija.VremePocetkaOperacije <= predlozeniPregled.VremePocetkaPregleda) &&
            (zakazanaOperacija.VremeKrajaOperacije >= predlozeniPregled.VremePocetkaPregleda))
        {
            return true;
        }

        return false;
    }

    public bool TerminSePreklapaKodLekara(string jmbgLekara, Pregled predlozeniPregled)
    {
        foreach (Pregled zakazaniPregled in SviBuduciPreglediKojeLekarIma(jmbgLekara))
        {
            if (PreklapanjeTerminaPregleda(predlozeniPregled, zakazaniPregled))
            {
                return true;
            }
        }

        foreach (Operacija zakazanaOperacija in bazaOperacija.SveSledeceOperacijeDatogLekara(jmbgLekara))
        {
            if (PreklapanjeTerminaPregledaSaOperacijom(predlozeniPregled, zakazanaOperacija))
            {
                return true;
            }
        }

        return false;
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

        System.DateTime pocetakIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
        System.DateTime krajIntervala = new System.DateTime(datum.Year, datum.Month, datum.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);
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
                // TODO : refaktorisati (moze se pozvati metoda PreklapanjeTerminaPregleda(Pregled predlozeniPregled, Pregled zakazaniPregled) )
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
        List<Pregled> sviPregledi = SviBuduciPregledi();
        BazaLekara bl = new BazaLekara();
        List<Lekar> lekari = bl.LekariOpstePrakse();

        //PROLAZIM KROZ SVE LEKARE OPSTE PRAKSE
        foreach (Lekar l in lekari)
        {
            //FORMIRANJE RADNOG VREMENA LEKARA ZA DAN PROSLEDJENOG PREGLEDA
            System.DateTime pocetakRadnogVremena = new System.DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month, pregled.VremePocetkaPregleda.Day,
                l.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, l.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
            System.DateTime krajRadnogVremena = new System.DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month, pregled.VremePocetkaPregleda.Day,
                l.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, l.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);

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

    // TODO OBRISATI OVO JER VEC POSTOJI U SERVISU
    public List<Pregled> SviBuduciPreglediKojePacijentIma(string jmbgPacijenta)
    {
        List<Pregled> pregledi = new List<Pregled>();

        foreach (Pregled pregled in SviBuduciPregledi())
        {
            if (pregled.Pacijent.Jmbg.Equals(jmbgPacijenta))
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

        foreach (Pregled p in SviBuduciPregledi())
        {
            if (p.Lekar.Jmbg.Equals(jmbgLekara) && p.VremeKrajaPregleda > DateTime.Now)
            {
                pregledi.Add(p);
            }
        }

        return pregledi;
    }

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
                    p.Pacijent = pacijent;
                    break;
                }
            }

            pregledi.Add(p);
            }

        return pregledi;
    }

    //OPTIMIZOVATI KOD
    public List<Pregled> SviBuduciPregledi()
    {
        List<Pregled> sviBuduciPregledi = new List<Pregled>();
        foreach (Pregled pregled in SviPregledi())
        {
            if (pregled.VremePocetkaPregleda > DateTime.Now.AddHours(-1))
            {
                sviBuduciPregledi.Add(pregled);
            }
        }

        return sviBuduciPregledi;
    }

    // TODO POSTOJI VEC U SERVISU I TREBA GA ODAVDE SKLONITI
    private bool MozeDaSeZakaze(Pregled noviPregled)
    {
        VremenskiInterval vremenskiIntervalNovogPregleda =
            new VremenskiInterval(noviPregled.VremePocetkaPregleda, noviPregled.VremeKrajaPregleda);

        if (!noviPregled.Lekar.TerminURadnomVremenuLekara(vremenskiIntervalNovogPregleda))
        {
            return false;
        }
        if (TerminSePreklapaKodLekara(noviPregled.Lekar.Jmbg, noviPregled))
        {
            return false;
        }

        return true;
    }

    public void ZakaziPregled(Pregled noviPregled)
    {
        if (!MozeDaSeZakaze(noviPregled))
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

        foreach (Pregled pregled in SviBuduciPregledi())
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

        foreach (Pregled pregled in SviBuduciPregledi())
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
        OtkaziPregled(stariPregled);
       ZakaziPregled(noviPregled);
    }

    public string FormatPisanjaPregleda(Pregled pregled){
        foreach (Lekar lekar in bazaLekara.SviLekari())
        {
            if (lekar.Jmbg.Equals(pregled.Lekar.Jmbg))
            {
                pregled.Lekar.Ordinacija = lekar.Ordinacija;
                break;
            }
        }
    
        return pregled.Pacijent.Jmbg + "#" + pregled.Lekar.Jmbg + "#" + pregled.VremePocetkaPregleda.ToString(vremenskiFormatPisanje)
            + "#" + pregled.VremeKrajaPregleda.ToString(vremenskiFormatPisanje) + "#" + pregled.Lekar.Ordinacija.Id;
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