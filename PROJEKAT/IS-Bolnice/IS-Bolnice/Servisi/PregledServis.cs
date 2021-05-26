using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class PregledServis
    {
        private readonly int MINUTI_TRAJANJA_PREGLEDA = 45;
        private readonly int DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA = 6;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_PREGLEDA_LEKARU = 7200;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_HITNIH_PREGLEDA = 60;

        private BazaPregleda bazaPregleda = new BazaPregleda();

        public bool ZakaziPregled(Pregled pregled)
        {
            if (MozeDaSeZakaze(pregled))
            {
                bazaPregleda.ZakaziPregled(pregled);
                return true;
            }

            return false;
        }
        public void IzmeniPregled(Pregled novi, Pregled stari)
        {
            bazaPregleda.IzmeniPregled(novi, stari);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            bazaPregleda.OtkaziPregled(pregled);
        }
        public void OdloziPregledStoPre(Pregled pomeraniPregled)
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

        public List<Pregled> GetsviPregledi()
        {
            return bazaPregleda.SviPregledi();
        }

        public List<Pregled> GetSviBuduciPregledi()
        {
            return bazaPregleda.SviBuduciPregledi();
        }

        public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
        {
            List<Pregled> sviPreglediLekara = new List<Pregled>();
            foreach (Pregled pregled in bazaPregleda.SviPregledi())
            {
                if (pregled.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    sviPreglediLekara.Add(pregled);
                }
            }

            return sviPreglediLekara;
        }

        public List<Pregled> GetSviBuduciPreglediSobe(string idSobe)
        {
            List<Pregled> preglediUSobi = new List<Pregled>();
            foreach (Pregled pregled in GetSviBuduciPregledi())
            {
                if (pregled.Lekar.Ordinacija.Id.Equals(idSobe))
                {
                    preglediUSobi.Add(pregled);
                }
            }
            return preglediUSobi;
        }

        public List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta)
        {
            List<Pregled> pregledi = new List<Pregled>();

            foreach (Pregled pregled in bazaPregleda.SviBuduciPregledi())
            {
                if (pregled.Pacijent.Jmbg.Equals(jmbgPacijenta))
                {
                    pregledi.Add(pregled);
                }
            }

            pregledi.Sort((x, y) => x.VremePocetkaPregleda.CompareTo(y.VremePocetkaPregleda));

            return pregledi;
        }

        public List<Pregled> GetSviBuduciPreglediLekara(string jmbgLekara)
        {
            List<Pregled> sviPregledi = new List<Pregled>();
            foreach (Pregled pregled in bazaPregleda.SviBuduciPregledi())
            {
                if (pregled.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    sviPregledi.Add(pregled);
                }
            }

            return sviPregledi;
        }

        public List<Pregled> GetDostupniTerminiPregledaLekaraUNarednomPeriodu(Lekar lekar)
        {
            List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar, MINUTI_TRAJANJA_PREGLEDA, MINUTI_INTERVALA_ZA_PREDLAGANJE_PREGLEDA_LEKARU);
            List<Pregled> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Pregled> slobodniTermini = SlobodniPreglediLekara(lekar, terminiURadnomVremenu);

            return slobodniTermini;
        }

        private List<Pregled> SviPredloziSkorasnjihPregleda(Lekar lekar, int minutiTrajanjaPregleda, int minutiIntervalaPredlaganja)
        {
            List<Pregled> sviSkorasnjiPregledi = new List<Pregled>();
            DateTime najbliziTermin = NajbliziTermin();


            for (int i = 0; i < minutiIntervalaPredlaganja; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                Pregled pregled = new Pregled()
                {
                    Lekar = lekar,
                    VremePocetkaPregleda = pocetakTermina,
                    VremeKrajaPregleda = pocetakTermina.AddMinutes(minutiTrajanjaPregleda)
                };
                sviSkorasnjiPregledi.Add(pregled);
            }
            return sviSkorasnjiPregledi;
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

        private List<Pregled> SviTerminiURadnomVremenuLekara(Lekar lekar, List<Pregled> pregledi)
        {
            List<Pregled> preglediURadnomVremenu = new List<Pregled>();

            foreach (Pregled pregled in pregledi)
            {
                // TODO: obrisati ovo formiranje intervala i u pregled dodati polje za interval
                VremenskiInterval termin = new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);

                if (lekar.TerminURadnomVremenuLekara(termin))
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

        public bool TerminSePreklapaKodLekara(string jmbgLekara, Pregled predlozeniPregled)
        {
            VremenskiInterval drugiTermin = new VremenskiInterval(predlozeniPregled.VremePocetkaPregleda,
                predlozeniPregled.VremeKrajaPregleda);

            foreach (Pregled zakazaniPregled in SviBuduciPreglediKojeLekarIma(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazaniPregled.VremePocetkaPregleda,
                    zakazaniPregled.VremeKrajaPregleda);

                if (prviTermin.DaLiSePreklapaSa(drugiTermin))
                {
                    return true;
                }

            }

            foreach (Operacija zakazanaOperacija in new OperacijaKontroler().GetSveSledeceOperacijeLekara(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (prviTermin.DaLiSePreklapaSa(drugiTermin))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Pregled> SviBuduciPreglediKojeLekarIma(string jmbgLekara)
        {
            List<Pregled> pregledi = new List<Pregled>();

            foreach (Pregled p in GetSviBuduciPregledi())
            {
                if (p.Lekar.Jmbg.Equals(jmbgLekara) && p.VremeKrajaPregleda > DateTime.Now)
                {
                    pregledi.Add(p);
                }
            }

            return pregledi;
        }

        public bool IzmeniPregled(DateTime stariDatum, string stariSat, string stariMinut, Pregled noviPregled)
        {
            BazaPregleda baza = new BazaPregleda();
            List<Pregled> lista = baza.SviBuduciPregledi();
            foreach (Pregled pregled in lista)
            {
                if (noviPregled.Pacijent.Jmbg.Equals(pregled.Pacijent.Jmbg) &&
                    pregled.VremePocetkaPregleda.Hour == Int32.Parse(stariSat) &&
                    pregled.VremePocetkaPregleda.Date.Equals(stariDatum))
                {
                    Pregled stariPregled = pregled;
                    baza.IzmeniPregled(noviPregled, stariPregled);
                    return true;
                }
            }
            return false;
        }

        public Pregled GetSledeciPregledKodLekara(string jmbg)
        {
            Pregled sledeciPregled = new Pregled();
            sledeciPregled.VremePocetkaPregleda = DateTime.MaxValue;
            foreach (Pregled pregled in GetSviBuduciPreglediLekara(jmbg))
            {
                if (pregled.VremePocetkaPregleda < sledeciPregled.VremePocetkaPregleda)
                {
                    sledeciPregled = pregled;
                }
            }

            return sledeciPregled;
        }

        public List<Pregled> ZauzetiHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast)
        {
            BazaLekara bazaLekara = new BazaLekara();
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

        public List<Pregled> SlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, int minutiTrajanjaPregleda)
        {
            BazaLekara bazaLekara = new BazaLekara();

            foreach (Lekar lekar in bazaLekara.LekariOdredjeneOblasti(prosledjenaOblast.Naziv))
            {
                List<Pregled> slobodniTerminiLekara = SlobodniHitniPreglediLekaraSaTrajanjem(lekar, minutiTrajanjaPregleda);
                if (slobodniTerminiLekara.Count() > 0)
                {
                    return slobodniTerminiLekara;
                }
            }
            return null;
        }

        private List<Pregled> SlobodniHitniPreglediLekaraSaTrajanjem(Lekar lekar, int minutiTrajanjePregleda)
        {
            List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar, minutiTrajanjePregleda, MINUTI_INTERVALA_ZA_PREDLAGANJE_HITNIH_PREGLEDA);
            List<Pregled> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Pregled> slobodniTermini = SlobodniPreglediLekara(lekar, terminiURadnomVremenu);

            return slobodniTermini;
        }

        //PROVERA DA LI PACIJENT VEC IMA ZAKAZANI PREGLED U TOM TERMINU
        public bool PacijentImaZakazanPregled(Pregled pregledZaProveru)
        {
            VremenskiInterval intervalPregledaZaProveru =
                new VremenskiInterval(pregledZaProveru.VremePocetkaPregleda, pregledZaProveru.VremeKrajaPregleda);

            foreach (Pregled pregled in GetSviBuduciPreglediPacijenta(pregledZaProveru.Pacijent.Jmbg))
            {
                VremenskiInterval intervalPregleda =
                    new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                if (intervalPregledaZaProveru.DaLiSePreklapaSa(intervalPregleda))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
