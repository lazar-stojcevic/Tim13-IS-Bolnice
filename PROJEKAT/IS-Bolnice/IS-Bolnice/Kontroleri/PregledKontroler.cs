using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class PregledKontroler
    {
        private PregledServis pregledServis = new PregledServis();

        public List<Pregled> GetSviPregledi()
        {
            return pregledServis.GetsviPregledi();
        }
        public List<Pregled> GetSviBuduciPregledi()
        {
            return pregledServis.GetSviBuduciPregledi();
        }

        public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
        {
            return pregledServis.GetSviPreglediLekara(jmbgLekara);
        }

        public List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta)
        {
            return pregledServis.GetSviBuduciPreglediPacijenta(jmbgPacijenta);
        }

        public List<Pregled> GetSviBuduciPreglediLekara(string jmbg)
        {
            return pregledServis.GetSviBuduciPreglediLekara(jmbg);
        }

        public Pregled GetSledeciPregledKodLekara(string jmbg)
        {
            return pregledServis.GetSledeciPregledKodLekara(jmbg);
        }

        public List<Pregled> GetDostupniTerminiPregledaLekara(Lekar lekar)
        {
            return pregledServis.GetDostupniTerminiPregledaLekara(lekar);
        }

        public bool IzmeniPregled(DateTime stariDatum, string stariSat, string stariMinut, Pregled noviPregled)
        {
            return pregledServis.IzmeniPregled(stariDatum, stariSat, stariMinut, noviPregled);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            pregledServis.OtkaziPregled(pregled);
        }

        public void ZakaziPregled(Pregled pregled)
        {
            pregledServis.ZakaziPregled(pregled);
        }

        public void IzmeniPregled(Pregled novi, Pregled stari)
        {
            pregledServis.IzmeniPregled(novi, stari);
        }

    }
}
