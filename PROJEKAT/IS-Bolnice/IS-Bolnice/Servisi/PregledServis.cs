using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class PregledServis
    {
        private readonly int MINUTI_TRAJANJA_PREGLEDA = 45;
        private readonly int DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA = 6;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_PREGLEDA_LEKARU = 5760;
        private readonly int MINUTI_PREDLAGANJA_ZA_3_DANA = 4320;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_HITNIH_PREGLEDA = 60;
        private readonly int MINUTI_INTERVALA_ZA_IZMENU_TERMINA_PREGLEDA = 30;


        private PreglediFajlRepozitorijum preglediFajlRepozitorijum = new PreglediFajlRepozitorijum();
        private ILekarRepozitorijum lekarRepo = new LekarFajlRepozitorijum();
        private BazaIzmena bazaIzmena = new BazaIzmena();

        public List<Pregled> SlobodniTerminiZaIzmenuPregledaPacijenta(Lekar lekar, DateTime datum)
        {
            List<Pregled> validni = new List<Pregled>();
            
            foreach (Pregled predlozeni in ListaSlobodnihPregledaTokomRadnogVremenaLekara(lekar, datum))
            {
                if (MozeDaSeZakaze(predlozeni))
                {
                    validni.Add(predlozeni);
                }
            }

            return validni;
        }

        private DateTime FormirajPocetakIntervalaZaIzmenu(DateTime datum)
        {
            return new System.DateTime(datum.Year, datum.Month, datum.Day, 0, 0, 0, 0);
        }

        private DateTime FormirajKrajIntervalaZaIzmenu(DateTime datum)
        {
            return new System.DateTime(datum.Year, datum.Month, datum.Day + 1, 0, 0, 0, 0);
        }

        private List<Pregled> ListaSlobodnihPregledaTokomRadnogVremenaLekara(Lekar lekar, DateTime datum)
        {
            List<Pregled> slobodni = new List<Pregled>();

            DateTime pocetakIntervala = FormirajPocetakIntervalaZaIzmenu(datum);
            DateTime krajIntervala = FormirajKrajIntervalaZaIzmenu(datum).AddMinutes(-30);

            while (pocetakIntervala <= krajIntervala)
            {
                Pregled p = new Pregled(null, lekar, pocetakIntervala, pocetakIntervala.AddMinutes(30));
                pocetakIntervala = pocetakIntervala.AddMinutes(MINUTI_INTERVALA_ZA_IZMENU_TERMINA_PREGLEDA);
                slobodni.Add(p);
            }

            return slobodni;
        }

        public bool ZakaziPregled(Pregled pregled)
        {
            if (MozeDaSeZakaze(pregled))
            {
                preglediFajlRepozitorijum.Sacuvaj(pregled);
                UpisiIzmenuUBazu(pregled.Pacijent.Jmbg);
                return true;
            }

            return false;
        }
        public void IzmeniPregled(Pregled noviPregled, Pregled stariPregled)
        {
            noviPregled.Id = stariPregled.Id;
            preglediFajlRepozitorijum.Izmeni(noviPregled);
            UpisiIzmenuUBazu(noviPregled.Pacijent.Jmbg);
        }

        private void UpisiIzmenuUBazu(string jmbgPacijenta)
        {
            Change izmena = new Change();
            DateTime sada = DateTime.Now;

            izmena.DateOfChange = sada;
            izmena.JmbgOfPatient = jmbgPacijenta;

            bazaIzmena.SaveChange(izmena);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            preglediFajlRepozitorijum.Obrisi(pregled.Id);
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
            return preglediFajlRepozitorijum.DobaviSve();
        }

        public List<Pregled> GetSviBuduciPregledi()
        {
            return preglediFajlRepozitorijum.SviBuduciPregledi();
        }

        public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
        {
            List<Pregled> sviPreglediLekara = new List<Pregled>();
            foreach (Pregled pregled in preglediFajlRepozitorijum.DobaviSve())
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

            foreach (Pregled pregled in preglediFajlRepozitorijum.SviBuduciPregledi())
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
            foreach (Pregled pregled in preglediFajlRepozitorijum.SviBuduciPregledi())
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

        public bool IzmeniPregled(Pregled noviPregled)
        {
            PreglediFajlRepozitorijum baza = new PreglediFajlRepozitorijum();
            baza.Izmeni(noviPregled);
            return true;
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
            List<Lekar> sviLekariOdredjeneOblasti = lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
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

        public List<Pregled> SlobodniPreglediLekaraOpstePrakseUNarednomPeriodu()
        {
            foreach (Lekar lekar in lekarRepo.LekariOpstePrakse())
            {
                List<Pregled> slobodniTerminiLekara =
                    SviPredloziSkorasnjihPregleda(lekar, MINUTI_TRAJANJA_PREGLEDA, MINUTI_PREDLAGANJA_ZA_3_DANA);
                List<Pregled> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, slobodniTerminiLekara);
                List<Pregled> slobodniTermini = SlobodniPreglediLekara(lekar, terminiURadnomVremenu);
                if (slobodniTermini.Count > 0)
                {
                    return slobodniTermini;
                }
            }

            return null;
        }

        public List<Pregled> SlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, int minutiTrajanjaPregleda)
        {
            foreach (Lekar lekar in lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv))
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
