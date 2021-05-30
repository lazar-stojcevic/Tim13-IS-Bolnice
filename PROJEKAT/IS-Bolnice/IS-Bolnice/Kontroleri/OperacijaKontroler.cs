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

        public List<Operacija> GetSveBuduceOperacije()
        {
            return operacijaServis.GetSveBuduceOperacije();
        }

        public List<Operacija> GetSveOperacijeLekara(string jmbgLekara)
        {
            return operacijaServis.GetSveOperacijeLekara(jmbgLekara);
        }
        public List<Operacija> GetSveBuduceOperacijeLekara(string jmbgLekara)
        {
            return operacijaServis.GetSveBuduceOperacijeLekara(jmbgLekara);
        }

        public List<Operacija> GetSveBuduceOperacijePacijenta(string jmbgPacijenta)
        {
            return operacijaServis.GetSveBuduceOperacijePacijenta(jmbgPacijenta);
        }

        public List<Operacija> GetSveBuduceOperacijeSale(string idSale)
        {
            return operacijaServis.GetSveBuduceOperacijeSale(idSale);
        }

        public List<Operacija> DostuptniTerminiLekaraZaDatuProstoriju(OperacijaDTO operacija)
        {
            return operacijaServis.DostuptniTerminiLekaraZaDatuProstoriju(operacija);
        }

        public bool IzmeniOperaciju(Operacija novaOperacija)
        {
            return operacijaServis.IzmeniOperaciju(novaOperacija);
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
