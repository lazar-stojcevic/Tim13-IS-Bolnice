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

        public List<Pregled> GetSviBuduciPregledi()
        {
            return pregledServis.GetSviBuduciPregledi();
        }

        public bool IzmeniPregled(DateTime stariDatum, string stariSat, string stariMinut, Pregled noviPregled)
        {
            return pregledServis.izmeniPregled(stariDatum, stariSat, stariMinut, noviPregled);
        }

        public List<Pregled> PonudjeniSlobodniTerminiLekara(string jmbgLekara)
        {
            return pregledServis.GetSlobodniTerminiLekara(jmbgLekara);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            pregledServis.OtkaziPregled(pregled);
        }

        public void ZakaziPregled(Pregled pregled)
        {
            pregledServis.ZakaziPregled(pregled);
        }

        public List<Pregled> GetSviBuduciPreglediLekara(string jmbg)
        {
            return pregledServis.GetSviBuduciPreglediLekara(jmbg);
        }

        public Pregled GetSledeciPregledKodLekara(string jmbg)
        {
            return pregledServis.GetSledeciPregledKodLekara(jmbg);
        }
    }
}
