using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.DTOs;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class OperacijaKontroler
    {
        private OperacijaServis operacijaServis = new OperacijaServis();

        public List<Operacija> GetSveOperacije()
        {
            return operacijaServis.GetSveOperacije();
        }
        public List<Operacija> GetSveSledeceOperacije()
        {
            return operacijaServis.GetSveSledeceOperacije();
        }

        public List<Operacija> GetSveOperacijeLekara(string jmbgLekara)
        {
            return operacijaServis.GetSveOperacijeLekara(jmbgLekara);
        }
        public List<Operacija> GetSveSledeceOperacijeLekara(string jmbgLekara)
        {
            return operacijaServis.GetSveSledeceOperacijeLekara(jmbgLekara);
        }

        public List<Operacija> GetSveSledeveOperacijePacijenta(string jmbgPacijenta)
        {
            return operacijaServis.GetSveSledeveOperacijePacijenta(jmbgPacijenta);
        }

        public List<Operacija> GetSveSledeceOperacijeSale(string idSale)
        {
            return operacijaServis.GetSveSledeceOperacijeSale(idSale);
        }

        public List<Operacija> DostuptniTerminiLekaraZaDatuProstoriju(OperacijaDTO operacija)
        {
            return operacijaServis.DostuptniTerminiLekaraZaDatuProstoriju(operacija);
        }

        public bool IzmeniOperaciju(Operacija novaOperacija)
        {
            return operacijaServis.IzmeniOperaciju(novaOperacija);
        }

        public void IzmeniOperaciju(Operacija nova, Operacija stara)
        {
            operacijaServis.IzmeniOperaciju(nova);
        }

        public bool ZakaziOperaciju(Operacija operacija)
        {
            return operacijaServis.ZakaziOperaciju(operacija);
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            operacijaServis.OtkaziOperaciju(operacija);
        }

        public void OdloziOperaciju(Operacija pomeranaOperacija)
        {
            operacijaServis.OdloziOperaciju(pomeranaOperacija);
        }

        public List<Operacija> ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(OblastLekara oblastLekara)
        {
            return operacijaServis.ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(oblastLekara);
        }

        public List<Operacija> SlobodneHitneOperacijeLekaraOdredjeneOblasti(OblastLekara oblastLekara,
            int minutiTrajanjaOperacije)
        {
            return operacijaServis.SlobodneHitneOperacijeLekaraOdredjeneOblasti(oblastLekara, minutiTrajanjaOperacije);
        }
    }
}
