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
    private static int BROJ_MINUTA_ZA_HITAN_TERMIN = 60;
    private static int DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA = 6;

    public List<Pregled> ZauzetiHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast)
    {
        List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
        List<Pregled> skorasnjiZauzetiPreglediLekara = new List<Pregled>();

        foreach (Lekar lekar in sviLekariOdredjeneOblasti)
        {
            List<Pregled> naredniPreglediLekara = SviBuduciPreglediKojeLekarIma(lekar.Jmbg);
            skorasnjiZauzetiPreglediLekara.AddRange(PreglediNarednihSatVremena(naredniPreglediLekara));
            if (skorasnjiZauzetiPreglediLekara.Count() > DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA)
            {
                return skorasnjiZauzetiPreglediLekara;
            }
        }
        return SortiranjeTerminaPoMogucstvuOdlaganja(skorasnjiZauzetiPreglediLekara);
    }

    private List<Pregled> PreglediNarednihSatVremena(List<Pregled> pregledi)
    {
        List<Pregled> preglediNarednihSatVremena = new List<Pregled>();
        DateTime trenutnoVreme = DateTime.Now;
        DateTime vremeZaSatVremena = trenutnoVreme.AddHours(1);
        foreach (Pregled pregled in pregledi)
        {
            if (pregled.VremePocetkaPregleda <= vremeZaSatVremena)
            {
                preglediNarednihSatVremena.Add(pregled);
            }
        }
        return preglediNarednihSatVremena;
    }

    private List<Pregled> SortiranjeTerminaPoMogucstvuOdlaganja(List<Pregled> pregledi)
    {
        List<int> vremenaOdlaganja = new List<int>();

        foreach (Pregled pregled in pregledi)
        {
            Pregled odlozeniPregled = new Pregled(pregled);

            int vremeOdlaganja = 10;
            while (!MozeDaSeZakaze(odlozeniPregled))
            {
                odlozeniPregled.VremePocetkaPregleda = odlozeniPregled.VremePocetkaPregleda.AddMinutes(vremeOdlaganja);
                odlozeniPregled.VremeKrajaPregleda = odlozeniPregled.VremeKrajaPregleda.AddMinutes(vremeOdlaganja);
                vremeOdlaganja += 10;
            }
            vremenaOdlaganja.Add(vremeOdlaganja);
        }
        return SortirajPregledePoVremenuOdlaganja(pregledi, vremenaOdlaganja);
    }

    private List<Pregled> SortirajPregledePoVremenuOdlaganja(List<Pregled> pregledi, List<int> odlaganja)
    {
        for (int i = 0; i < odlaganja.Count - 1; i++)
        {
            for (int j = 0; j < odlaganja.Count - i - 1; j++)
            {
                if (odlaganja[j] > odlaganja[j + 1])
                {
                    int temp = odlaganja[j];
                    odlaganja[j] = odlaganja[j + 1];
                    odlaganja[j + 1] = temp;

                    Pregled tempPregled = pregledi[j];
                    pregledi[j] = pregledi[j + 1];
                    pregledi[j + 1] = tempPregled;
                }
            }
        }
        return pregledi;
    }

    public List<Pregled> SlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, double trajanjePregleda)
    {
        List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);

        foreach (Lekar lekar in sviLekariOdredjeneOblasti)
        {
            List<Pregled> slobodniTerminiLekara = SlobodniHitniPreglediLekaraSaTrajanjem(lekar, trajanjePregleda);
            if (slobodniTerminiLekara.Count() > 0)
            {
                return slobodniTerminiLekara;
            }
        }
        return null;
    }

    // TRAJANJE TERMINA PREGLEDA JE IZRAZENO U SATIMA
    private List<Pregled> SlobodniHitniPreglediLekaraSaTrajanjem(Lekar lekar, double trajanjePregleda)
    {
        List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar, trajanjePregleda);
        List<Pregled> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
        List<Pregled> slobodniTermini = SlobodniPreglediLekara(lekar, terminiURadnomVremenu);

        return slobodniTermini;
    }

    private List<Pregled> SviPredloziSkorasnjihPregleda(Lekar lekar, double trajanjePregleda)
    {
        List<Pregled> sviSkorasnjiPregledi = new List<Pregled>();
        DateTime najbliziTermin = NajbliziTermin();

        for (int i = 0; i < BROJ_MINUTA_ZA_HITAN_TERMIN; i += 5)
        {
            DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

            Pregled pregled = new Pregled();
            pregled.Lekar = lekar;
            pregled.VremePocetkaPregleda = pocetakTermina;
            pregled.VremeKrajaPregleda = pregled.VremePocetkaPregleda.AddHours(trajanjePregleda);
            sviSkorasnjiPregledi.Add(pregled);
        }
        return sviSkorasnjiPregledi;
    }

    private List<Pregled> SviTerminiURadnomVremenuLekara(Lekar lekar, List<Pregled> pregledi)
    {
        DateTime najbliziTermin = NajbliziTermin();
        List<Pregled> preglediURadnomVremenu = new List<Pregled>();

        // ukoliko lekaru radno vreme pocinje u jednom danu i traje preko tog dana
        int razlika = lekar.KrajRadnogVremena.Day - lekar.PocetakRadnogVremena.Day;

        DateTime pocetakRadnogVremena = new DateTime(najbliziTermin.Year, najbliziTermin.Month, najbliziTermin.Day, lekar.PocetakRadnogVremena.Hour, lekar.PocetakRadnogVremena.Minute, 0, 0);
        DateTime krajRadnogVremena = new DateTime(najbliziTermin.Year, najbliziTermin.Month, najbliziTermin.Day + razlika, lekar.KrajRadnogVremena.Hour, lekar.KrajRadnogVremena.Minute, 0, 0);
        foreach (Pregled pregled in pregledi)
        {
            if (pocetakRadnogVremena <= pregled.VremePocetkaPregleda && krajRadnogVremena >= pregled.VremeKrajaPregleda)
            {
                preglediURadnomVremenu.Add(pregled);
            }
        }
        return preglediURadnomVremenu;
    }

    private List<Pregled> SlobodniPreglediLekara(Lekar lekar, List<Pregled> pregledi)
    {
        List<Pregled> slobodniTermini = new List<Pregled>();
        foreach (Pregled pregled in pregledi)
        {
            if (!TerminSePreklapaKodLekara(lekar.Jmbg, pregled))
            {
                slobodniTermini.Add(pregled);
            }
        }
        return slobodniTermini;
    }

    private DateTime NajbliziTermin()
    {
        DateTime najbliziTermin = DateTime.Now;
        najbliziTermin = najbliziTermin.AddMinutes(1);
        while (najbliziTermin.Minute % 5 != 0)
        {
            najbliziTermin = najbliziTermin.AddMinutes(1);
        }
        return najbliziTermin;
    }

    //OVA METODA VRACA LISTU SLOBODNIH PREGLEDA KOJI MOGU DA SE ZAKAZU KOD PROSLEDJENOG LEKARA
    public List<Pregled> PonudjeniSlobodniPreglediLekara(string jmbgLekara)
    {
        Lekar lekar = PronadjiLekara(jmbgLekara);

        List<Pregled> validni = new List<Pregled>();

        foreach (Pregled predlozeniPregled in ListaSlobodnihTerminaPregledaTokomRadnogVremenaLekara(lekar))
        {
            if (TerminSePreklapaKodLekara(jmbgLekara, predlozeniPregled))
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
            return true;
        }
        if (predlozeniPregled.VremePocetkaPregleda > zakazaniPregled.VremePocetkaPregleda && predlozeniPregled.VremePocetkaPregleda < zakazaniPregled.VremeKrajaPregleda)
        {
            return true;
        }
        if (predlozeniPregled.VremeKrajaPregleda > zakazaniPregled.VremePocetkaPregleda && predlozeniPregled.VremeKrajaPregleda < zakazaniPregled.VremeKrajaPregleda)
        {
            return true;
        }
        return false;
    }

    public bool PreklapanjeTerminaOperacija(Pregled predlozeniPregled, Operacija zakazanaOperacija)
    {
        if (predlozeniPregled.VremePocetkaPregleda == zakazanaOperacija.VremePocetkaOperacije)
        {
            return true;
        }
        if (predlozeniPregled.VremePocetkaPregleda > zakazanaOperacija.VremePocetkaOperacije && predlozeniPregled.VremePocetkaPregleda < zakazanaOperacija.VremeKrajaOperacije)
        {
            return true;
        }
        if (predlozeniPregled.VremeKrajaPregleda > zakazanaOperacija.VremePocetkaOperacije && predlozeniPregled.VremeKrajaPregleda < zakazanaOperacija.VremeKrajaOperacije)
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
            if (PreklapanjeTerminaOperacija(predlozeniPregled, zakazanaOperacija))
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
            if (p.Lekar.Jmbg.Equals(jmbgLekara) && p.VremeKrajaPregleda > DateTime.Now)
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

    private bool MozeDaSeZakaze(Pregled noviPregled)
    {
        foreach (Pregled zakazani in SviPregledi())
        {
            if (noviPregled.Lekar.Jmbg == zakazani.Lekar.Jmbg)
            {
                if (zakazani.VremePocetkaPregleda == noviPregled.VremePocetkaPregleda)
                {
                    return false;
                }
                else if (noviPregled.VremePocetkaPregleda > zakazani.VremePocetkaPregleda && noviPregled.VremePocetkaPregleda < zakazani.VremeKrajaPregleda)
                {
                    return false;
                }
                else if (zakazani.VremePocetkaPregleda < noviPregled.VremeKrajaPregleda && noviPregled.VremeKrajaPregleda < zakazani.VremeKrajaPregleda)
                {
                    return false;
                }
            }
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

    public void OdloziPregled(Pregled pomeraniPregled)
    {
        Pregled pregledZaOtkazivanje = new Pregled(pomeraniPregled);

        double vremeOdlaganja = 10;
        do
        {
            pomeraniPregled.VremePocetkaPregleda = pomeraniPregled.VremePocetkaPregleda.AddMinutes(vremeOdlaganja);
            pomeraniPregled.VremeKrajaPregleda = pomeraniPregled.VremeKrajaPregleda.AddMinutes(vremeOdlaganja);
            vremeOdlaganja += 10;
        } while (!MozeDaSeZakaze(pomeraniPregled));

        OtkaziPregled(pregledZaOtkazivanje);
        ZakaziPregled(pomeraniPregled);
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