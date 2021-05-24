using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class PregledServis
    {
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

        public List<Pregled> GetSlobodniTerminiLekara(string jmbgLekara)
        {
            List<Pregled> preglediOvogLekara = new List<Pregled>();

            foreach (Pregled pregled in bazaPregleda.PonudjeniSlobodniPreglediLekara(jmbgLekara))
            {
                if (pregled.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    preglediOvogLekara.Add(pregled);
                }   
            }

            return preglediOvogLekara;
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
