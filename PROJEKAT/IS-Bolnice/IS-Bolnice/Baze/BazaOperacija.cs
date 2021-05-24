using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class BazaOperacija
{
    private BazaLekara bazaLekara = new BazaLekara();
    private BazaBolnica bazaBolnica = new BazaBolnica();
    public string fileLocation;
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    private static int BROJ_MINUTA_ZA_HITAN_TERMIN = 60;
    private static int DOVOLJAN_BROJ_ZAKAZANIH_OPERACIJA = 6;

    public List<Operacija> ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(OblastLekara prosledjenaOblast)
    {
        List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
        List<Operacija> skorasnjeOperacijeLekara = new List<Operacija>();

        foreach (Lekar lekar in sviLekariOdredjeneOblasti)
        {
            List<Operacija> naredneOperacijeLekara = SveSledeceOperacijeZaLekara(lekar.Jmbg);
            List<Operacija> operacijeKojeNisuHitne = SveOperacijeKojeNisuHitne(naredneOperacijeLekara);
            skorasnjeOperacijeLekara.AddRange(OperacijeNarednihSatVremena(operacijeKojeNisuHitne));
            if (skorasnjeOperacijeLekara.Count > DOVOLJAN_BROJ_ZAKAZANIH_OPERACIJA)
            {
                return skorasnjeOperacijeLekara;
            }
        }
        return SortiranjeTerminaPoMogucstvuOdlaganja(skorasnjeOperacijeLekara);
    }

    private List<Operacija> SortiranjeTerminaPoMogucstvuOdlaganja(List<Operacija> operacije)
    {
        List<int> vremenaOdlaganja = new List<int>();

        foreach (Operacija operacija in operacije)
        {
            Operacija odlozenaOperacija = new Operacija(operacija);

            int vremeOdlaganja = 10;
            while (!MozeDaSeZakaze(odlozenaOperacija))
            {
                odlozenaOperacija.VremePocetkaOperacije = odlozenaOperacija.VremePocetkaOperacije.AddMinutes(vremeOdlaganja);
                odlozenaOperacija.VremeKrajaOperacije = odlozenaOperacija.VremeKrajaOperacije.AddMinutes(vremeOdlaganja);
                vremeOdlaganja += 10;
            }
            vremenaOdlaganja.Add(vremeOdlaganja);
        }
        return SortirajOperacijePoVremenuOdlaganja(operacije, vremenaOdlaganja);
    }

    private List<Operacija> SortirajOperacijePoVremenuOdlaganja(List<Operacija> operacije, List<int> odlaganja)
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

                    Operacija tempOperacija = operacije[j];
                    operacije[j] = operacije[j + 1];
                    operacije[j + 1] = tempOperacija;
                }
            }
        }
        return operacije;
    }

    private List<Operacija> SveOperacijeKojeNisuHitne(List<Operacija> operacije)
    {
        List<Operacija> operacijeKojeNisuHitne = new List<Operacija>();

        foreach (Operacija operacija in operacije)
        {
            if (!operacija.Hitna)
            {
                operacijeKojeNisuHitne.Add(operacija);
            }
        }

        return operacijeKojeNisuHitne;
    }

    private bool MozeDaSeZakaze(Operacija operacija)
    {
        if (TerminSePreklapaKodLekara(operacija.Lekar.Jmbg, operacija))
        {
            return false;
        }
        if (!OperacijaURadnomVremenuLekara(operacija.Lekar, operacija))
        {
            return false;
        }

        return true;
    }

    private List<Operacija> OperacijeNarednihSatVremena(List<Operacija> operacije)
    {
        List<Operacija> operacijeiNarednihSatVremena = new List<Operacija>();
        DateTime trenutnoVreme = DateTime.Now;
        DateTime vremeZaSatVremena = trenutnoVreme.AddHours(1);
        foreach (Operacija operacija in operacije)
        {
            if (operacija.VremePocetkaOperacije <= vremeZaSatVremena)
            {
                operacijeiNarednihSatVremena.Add(operacija);
            }
        }
        return operacijeiNarednihSatVremena;
    }

    public List<Operacija> SlobodneHitneOperacijeLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, double trajanjeOperacije)
    {
        List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);

        foreach (Lekar lekar in sviLekariOdredjeneOblasti)
        {
            List<Operacija> slobodniTerminiLekara = SlobodneHitneOperacijeLekaraSaTrajanjem(lekar, trajanjeOperacije);
            if (slobodniTerminiLekara.Count > 0)
            {
                return slobodniTerminiLekara;
            }
        }
        return null;
    }

    public List<Operacija> SveOperacijeUOdabranojSali(string idSale)
    {
        List<Operacija> sveOperacije = SveSledeceOperacije();
        List<Operacija> operacijeUOdabranojSali = new List<Operacija>();
        foreach (Operacija operacija in sveOperacije)
        {
            if (operacija.Soba.Id.Equals(idSale))
            {
                operacijeUOdabranojSali.Add(operacija);
            }
        }
        return operacijeUOdabranojSali;
    }

    // TRAJANJE TERMINA PREGLEDA JE IZRAZENO U SATIMA
    private List<Operacija> SlobodneHitneOperacijeLekaraSaTrajanjem(Lekar lekar, double trajanjePregleda)
    {
        List<Operacija> sviSkorasnjiTermini = SviPredloziSkorasnjihOperacija(lekar, trajanjePregleda);
        List<Operacija> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
        List<Operacija> slobodniTermini = SlobodneOperacijeLekara(lekar, terminiURadnomVremenu);

        return slobodniTermini;
    }

    // TRAJANJE TERMINA PREGLEDA JE IZRAZENO U MINUTIMA
    public List<Operacija> SlobodneOperacijeLekaraUNarednomPeriodu(Lekar lekar, int trajanjePregleda, string idSale)
    {
        List<Operacija> sviSkorasnjiTermini = SviPredloziSkorasnjihOperacija2(lekar, trajanjePregleda, idSale);
        List<Operacija> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
        List<Operacija> slobodniTermini = SlobodneOperacijeLekara(lekar, terminiURadnomVremenu);

        return slobodniTermini;
    }

    private List<Operacija> SviPredloziSkorasnjihOperacija2(Lekar lekar, int trajanjeOperacije, string idSale)
    {
        List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
        DateTime najbliziTermin = NajbliziTermin();
        

        for (int i = 0; i < 7200; i += 10)
        {
            DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

            Operacija operacija = new Operacija()
            {
                Lekar = lekar,
                VremePocetkaOperacije = pocetakTermina,
                VremeKrajaOperacije = pocetakTermina.AddMinutes(trajanjeOperacije),
                Soba = bazaBolnica.GetSobaById(idSale),
                Hitna = true
            };
                sveSkorasnjeOperacije.Add(operacija);
        }
        return sveSkorasnjeOperacije;
    }

    private List<Operacija> SviPredloziSkorasnjihOperacija(Lekar lekar, double trajanjeOperacije)
    {
        List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
        DateTime najbliziTermin = NajbliziTermin();

        for (int i = 0; i < BROJ_MINUTA_ZA_HITAN_TERMIN; i += 5)
        {
            DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

            foreach (Soba sala in bazaBolnica.SveOperacioneSaleOveBolnice())
            {
                Operacija operacija = new Operacija()
                {
                    Lekar = lekar,
                    VremePocetkaOperacije = pocetakTermina,
                    VremeKrajaOperacije = pocetakTermina.AddHours(trajanjeOperacije),
                    Soba = sala,
                    Hitna = false
                };
                sveSkorasnjeOperacije.Add(operacija);
            }
        }
        return sveSkorasnjeOperacije;
    }

    private List<Operacija> SviTerminiURadnomVremenuLekara(Lekar lekar, List<Operacija> operacije)
    {
        List<Operacija> operacijeURadnomVremenu = new List<Operacija>();

        foreach (Operacija operacija in operacije)
        {
            // TODO: obrisati ovo formiranje intervala i u operaciju dodati polje za interval
            VremenskiInterval termin = new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
            
            if (lekar.TerminURadnomVremenuLekara(termin))
            {
                operacijeURadnomVremenu.Add(operacija);
            }
        }
        return operacijeURadnomVremenu;
    }

    private bool OperacijaURadnomVremenuLekara(Lekar lekar, Operacija operacija)
    {
        // TODO: obrisati ovo formiranje intervala i u pregled dodati polje za interval
        VremenskiInterval termin = new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);

        if (lekar.TerminURadnomVremenuLekara(termin))
        {
            return true;
        }
        return false;
    }

    private List<Operacija> SlobodneOperacijeLekara(Lekar lekar, List<Operacija> operacije)
    {
        List<Operacija> slobodniTermini = new List<Operacija>();
        foreach (Operacija operacija in operacije)
        {
            if (!TerminSePreklapaKodLekara(lekar.Jmbg, operacija))
            {
                slobodniTermini.Add(operacija);
            }
        }
        return slobodniTermini;
    }

    private bool TerminSePreklapaKodLekara(string jmbgLekara, Operacija predlozenaOperacija)
    {
        BazaPregleda bazaPregleda = new BazaPregleda();
        foreach (Pregled zakazaniPregled in bazaPregleda.SviBuduciPreglediKojeLekarIma(jmbgLekara))
        {
            if (PreklapanjeTerminaOperacijeSaPregledom(predlozenaOperacija, zakazaniPregled))
            {
                return true;
            }
        }

        foreach (Operacija zakazanaOperacija in SveSledeceOperacijeDatogLekara(jmbgLekara))
        {
            if (predlozenaOperacija.Soba.Jednaka(zakazanaOperacija.Soba) && PreklapanjeTerminaOperacija(predlozenaOperacija, zakazanaOperacija))
            {
                return true;
            }
        }

        return false;
    }

    private bool PreklapanjeTerminaOperacijeSaPregledom(Operacija predlozenaOperacija, Pregled zakazaniPregled)
    {
        if ((predlozenaOperacija.VremePocetkaOperacije <= zakazaniPregled.VremePocetkaPregleda) &&
            (predlozenaOperacija.VremeKrajaOperacije >= zakazaniPregled.VremePocetkaPregleda))
        {
            return true;
        }

        if ((predlozenaOperacija.VremePocetkaOperacije <= zakazaniPregled.VremePocetkaPregleda) &&
            (zakazaniPregled.VremeKrajaPregleda <= predlozenaOperacija.VremeKrajaOperacije))
        {
            return true;
        }

        if ((zakazaniPregled.VremePocetkaPregleda <= predlozenaOperacija.VremePocetkaOperacije) &&
            (predlozenaOperacija.VremeKrajaOperacije) <= zakazaniPregled.VremeKrajaPregleda)
        {
            return true;
        }

        if ((zakazaniPregled.VremePocetkaPregleda <= predlozenaOperacija.VremePocetkaOperacije) &&
            (zakazaniPregled.VremeKrajaPregleda >= predlozenaOperacija.VremePocetkaOperacije))
        {
            return true;
        }

        return false;
    }

    private bool PreklapanjeTerminaOperacija(Operacija predlozenaOperacija, Operacija zakazanaOperacija)
    {
        if ((predlozenaOperacija.VremePocetkaOperacije <= zakazanaOperacija.VremePocetkaOperacije) &&
            (predlozenaOperacija.VremeKrajaOperacije >= zakazanaOperacija.VremePocetkaOperacije))
        {
            return true;
        }

        if ((predlozenaOperacija.VremePocetkaOperacije <= zakazanaOperacija.VremePocetkaOperacije) &&
            (zakazanaOperacija.VremeKrajaOperacije <= predlozenaOperacija.VremeKrajaOperacije))
        {
            return true;
        }

        if ((zakazanaOperacija.VremePocetkaOperacije <= predlozenaOperacija.VremePocetkaOperacije) &&
            (predlozenaOperacija.VremeKrajaOperacije) <= zakazanaOperacija.VremeKrajaOperacije)
        {
            return true;
        }

        if ((zakazanaOperacija.VremePocetkaOperacije <= predlozenaOperacija.VremePocetkaOperacije) &&
            (zakazanaOperacija.VremeKrajaOperacije >= predlozenaOperacija.VremePocetkaOperacije))
        {
            return true;
        }

        return false;
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


    private static bool PreklapanjeTerminaUSali(string idSale, Operacija predlozeni, Operacija operacija)
    {
        return operacija.Soba.Id.Equals(idSale)
                            && ((predlozeni.VremePocetkaOperacije > operacija.VremePocetkaOperacije && predlozeni.VremePocetkaOperacije < operacija.VremeKrajaOperacije)
                            || (predlozeni.VremeKrajaOperacije > operacija.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije < operacija.VremeKrajaOperacije)
                            || (predlozeni.VremePocetkaOperacije == operacija.VremePocetkaOperacije));
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
                o.VremePocetkaOperacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                o.VremeKrajaOperacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                o.Pacijent.Jmbg = delovi[2];
                foreach (Pacijent p in pacijenti)
                {
                    if (o.Pacijent.Jmbg.Equals(p.Jmbg))
                    {
                        o.Pacijent = p;
                        break;
                    }
                }

                foreach (Bolnica bolnica in bolnice)
                {
                    foreach (Soba s in bolnica.Soba)
                    {
                        if (delovi[4].Equals(s.Id))
                        {
                            o.Soba = s;
                            break;
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

                if (o.VremePocetkaOperacije > DateTime.Now.AddHours(-1))
                {
                    ret.Add(o);
                }
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

    public void OdloziOperaciju(Operacija pomeranaOperacija)
    {
        Operacija operacijaZaOtkazivanje = new Operacija(pomeranaOperacija);

        double vremeOdlaganja = 10;
        do
        {
            pomeranaOperacija.VremePocetkaOperacije = pomeranaOperacija.VremePocetkaOperacije.AddMinutes(vremeOdlaganja);
            pomeranaOperacija.VremeKrajaOperacije = pomeranaOperacija.VremeKrajaOperacije.AddMinutes(vremeOdlaganja);
            vremeOdlaganja += 10;
        } while (!MozeDaSeZakaze(pomeranaOperacija));

        OtkaziOperaciju(operacijaZaOtkazivanje);
        ZakaziOperaciju(pomeranaOperacija);
    }

    public void IzmeniOperaciju(Operacija novaOperacija, Operacija staraOperacija)
   {
        OtkaziOperaciju(staraOperacija);
        ZakaziOperaciju(novaOperacija);
   }

    public List<Operacija> SveSledeceOperacijeZaLekara(string jmbgLekara)
    {
        List<Operacija> sledeceOperacije = new List<Operacija>();

        foreach (Operacija operacija in SveSledeceOperacije())
        {
            if (operacija.Lekar.Jmbg.Equals(jmbgLekara) && operacija.VremeKrajaOperacije > DateTime.Now)
            {
                sledeceOperacije.Add(operacija);
            }
        }
        return sledeceOperacije;
    }
}