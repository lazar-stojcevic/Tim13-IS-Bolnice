using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class OperacijaKontroler
    {
        private OperacijaServis operacijaServis = new OperacijaServis();

        public List<Operacija> GetSveSledeceOperacije()
        {
            return operacijaServis.GetSveSledeceOperacije();
        }

        public List<Operacija> SveSledeceOperacijeZaLekara(string jmbgLekara)
        {
            return operacijaServis.SveSledeceOperacijeZaLekara(jmbgLekara);
        }

        public List<Operacija> GetSveSledeveOperacijePacijenta(string jmbgPacijenta)
        {
            return operacijaServis.GetSveSledeveOperacijePacijenta(jmbgPacijenta);
        }

        public List<Operacija> GetSveSledeceOperacijeSobe(string idSale)
        {
            return operacijaServis.GetSveSledeceOperacijeSobe(idSale);
        }

        public List<Operacija> DostuptniTerminiLekaraZaDatuProstoriju(string jmbgLekara, string idSale, int duzinaTrajanja)
        {
            return operacijaServis.DostuptniTerminiLekaraZaDatuProstoriju(jmbgLekara, idSale, duzinaTrajanja);
        }

        public bool IzmeniOperaciju(DateTime stariDatum, string stariSat, string stariMinut, Operacija novaOperacija)
        {
            return operacijaServis.izmeniOperaciju(stariDatum, stariSat, stariMinut, novaOperacija);
        }

        public void IzmeniOperaciju(Operacija nova, Operacija stara)
        {
            operacijaServis.IzmeniOperaciju(nova, stara);
        }

        public void ZakaziOperaciju(Operacija operacija)
        {
            operacijaServis.ZakaziOperaciju(operacija);
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            operacijaServis.OtkaziOperaciju(operacija);
        }
    }
}
