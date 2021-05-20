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

        public bool IzmeniOperaciju(DateTime stariDatum, string stariSat, string stariMinut, Operacija novaOperacija)
        {
            return operacijaServis.izmeniOperaciju(stariDatum, stariSat, stariMinut, novaOperacija);
        }

        public void ZakaziOperaciju(Operacija operacija)
        {
            operacijaServis.ZakaziOperaciju(operacija);
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            operacijaServis.OtkaziOperaciju(operacija);
        }

        public List<Operacija> SveSledeceOperacijeZaLekara(string jmbgLekara)
        {
           return operacijaServis.SveSledeceOperacijeZaLekara(jmbgLekara);
        }
    }
}
