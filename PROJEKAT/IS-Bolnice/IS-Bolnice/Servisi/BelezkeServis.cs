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

        public void IzmeniBelezku(Beleska staraBeleska, Beleska novaBeleska)
        {
            bazaBelezaka.IzmeniBelezku(staraBeleska, novaBeleska);
        }

        public void SacuvajBelezku(Beleska beleska)
        {
            bazaBelezaka.SacuvajBelezku(beleska);
        }

        public List<Beleska> SveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            List<Beleska> pacijentoveBelezke = new List<Beleska>();

            foreach (Beleska belezka in bazaBelezaka.SveBelezke())
            {
                if (TrajeLiBelezka(belezka) && belezka.Pacijent.Jmbg == jmbgPacijenta)
                {
                    pacijentoveBelezke.Add(belezka);
                }
            }

            return pacijentoveBelezke;
        }

        public void ObrisiBelezku(Beleska beleska)
        {
            bazaBelezaka.ObrisiBelezku(beleska);
        }

        private bool TrajeLiBelezka(Beleska beleska)
        {
            DateTime vremeKrajaTrajanjaBelezke = beleska.VremePocetkaVazenja.AddDays(beleska.PeriodVazenja);

            if (vremeKrajaTrajanjaBelezke > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}
