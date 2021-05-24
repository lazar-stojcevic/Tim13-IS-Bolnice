using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class PregledServis
    {
        private static int MINUTI_TRAJANJA_PREGLEDA = 45;

        private BazaPregleda bazaPregleda = new BazaPregleda();

        public void ZakaziPregled(Pregled pregled)
        {
            bazaPregleda.ZakaziPregled(pregled);
        }
        public List<Pregled> GetSviBuduciPregledi()
        {
            return bazaPregleda.SviBuduciPregledi();
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

        public List<Pregled> GetDostupniTerminiPregledaLekara(Lekar lekar)
        {
            return SlobodniPreglediLekaraUNarednomPeriodu(lekar);
        }

        public List<Pregled> SlobodniPreglediLekaraUNarednomPeriodu(Lekar lekar)
        {
            List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar);
            List<Pregled> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Pregled> slobodniTermini = SlobodniPreglediLekara(lekar, terminiURadnomVremenu);

            return slobodniTermini;
        }

        private List<Pregled> SviPredloziSkorasnjihPregleda(Lekar lekar)
        {
            List<Pregled> sviSkorasnjiPregledi = new List<Pregled>();
            DateTime najbliziTermin = NajbliziTermin();


            for (int i = 0; i < 7200; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                Pregled pregled = new Pregled()
                {
                    Lekar = lekar,
                    VremePocetkaPregleda = pocetakTermina,
                    VremeKrajaPregleda = pocetakTermina.AddMinutes(MINUTI_TRAJANJA_PREGLEDA)
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

                if (PreklapanjeTermina(prviTermin, drugiTermin))
                {
                    return true;
                }

            }

            foreach (Operacija zakazanaOperacija in new BazaOperacija().SveSledeceOperacijeDatogLekara(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (PreklapanjeTermina(prviTermin, drugiTermin))
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

        public bool PreklapanjeTermina(VremenskiInterval predlozen, VremenskiInterval upitan)
        {
            return predlozen.DaLiSePreklapaSa(upitan);
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

        public void IzmeniPregled(Pregled novi, Pregled stari)
        {
            bazaPregleda.IzmeniPregled(novi, stari);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            bazaPregleda.OtkaziPregled(pregled);
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

    }
}
