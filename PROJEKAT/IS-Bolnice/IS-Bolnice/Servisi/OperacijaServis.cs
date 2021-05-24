using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class OperacijaServis
    {
        private BazaOperacija bazaOperacija = new BazaOperacija();

        public List<Operacija> GetSveSledeceOperacije()
        {
            return bazaOperacija.SveSledeceOperacije();
        }

        public List<Operacija> GetSveSledeveOperacijePacijenta(string jmbgPacijenta)
        {
            List<Operacija> operacijePacijenta = new List<Operacija>();

            foreach (Operacija o in bazaOperacija.SveSledeceOperacije())
            {
                if (o.Pacijent.Jmbg.Equals(jmbgPacijenta))
                {
                    operacijePacijenta.Add(o);
                }
            }

            return operacijePacijenta;
        }

        public List<Operacija> GetSveSledeceOperacijeSobe(string idSale)
        {
            List<Operacija> operacijeSale = new List<Operacija>();
            foreach (Operacija operacija in bazaOperacija.SveSledeceOperacije())
            {
                if (operacija.Soba.Id.Equals(idSale))
                {
                    operacijeSale.Add(operacija);
                }
            }

            return operacijeSale;
        }

        public bool izmeniOperaciju(DateTime stariDatum, string stariSat, string stariMinut, Operacija novaOperacija)
        {
            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            foreach (Operacija operacija in lista)
            {
                if (novaOperacija.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) &&
                    operacija.VremePocetkaOperacije.Hour == Int32.Parse(stariSat) &&
                    operacija.VremePocetkaOperacije.Date.Equals(stariDatum))
                {
                    Operacija staraOperacija = operacija;
                    baza.IzmeniOperaciju(novaOperacija, staraOperacija);
                    return true;
                }
            }
            return false;
        }

        public void IzmeniOperaciju(Operacija nova, Operacija stara)
        {
            bazaOperacija.IzmeniOperaciju(nova, stara);
        }

        public void ZakaziOperaciju(Operacija operacija)
        {
            bazaOperacija.ZakaziOperaciju(operacija);
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            bazaOperacija.OtkaziOperaciju(operacija);
        }

        public List<Operacija> SveSledeceOperacijeZaLekara(string jmbgLekara)
        {
            List<Operacija> ret = new List<Operacija>();
            foreach (Operacija operacija in bazaOperacija.SveSledeceOperacije())
            {
                if (operacija.Lekar.Jmbg.Equals(jmbgLekara) && operacija.VremePocetkaOperacije > DateTime.Now)
                {
                    ret.Add(operacija);
                }
            }
            return ret;
        }
    }
}
