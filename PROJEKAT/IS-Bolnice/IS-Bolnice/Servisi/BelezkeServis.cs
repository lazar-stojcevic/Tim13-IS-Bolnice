using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class BelezkeServis
    {
        private BazaBelezaka bazaBelezaka = new BazaBelezaka();

        public void IzmeniBelezku(Belezka staraBelezka, Belezka novaBelezka)
        {
            bazaBelezaka.IzmeniBelezku(staraBelezka, novaBelezka);
        }

        public void SacuvajBelezku(Belezka belezka)
        {
            bazaBelezaka.SacuvajBelezku(belezka);
        }

        public List<Belezka> SveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            List<Belezka> pacijentoveBelezke = new List<Belezka>();

            foreach (Belezka belezka in bazaBelezaka.SveBelezke())
            {
                if (TrajeLiBelezka(belezka) && belezka.Pacijent.Jmbg == jmbgPacijenta)
                {
                    pacijentoveBelezke.Add(belezka);
                }
            }

            return pacijentoveBelezke;
        }

        public void ObrisiBelezku(Belezka belezka)
        {
            bazaBelezaka.ObrisiBelezku(belezka);
        }

        private bool TrajeLiBelezka(Belezka belezka)
        {
            DateTime vremeKrajaTrajanjaBelezke = belezka.VremePocetkaVazenja.AddDays(belezka.PeriodVazenja);

            if (vremeKrajaTrajanjaBelezke > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}
