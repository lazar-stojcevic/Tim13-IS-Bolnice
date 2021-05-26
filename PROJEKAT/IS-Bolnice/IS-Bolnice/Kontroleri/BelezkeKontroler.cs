using IS_Bolnice.Model;
using IS_Bolnice.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Kontroleri
{
    class BelezkeKontroler
    {
        private BelezkeServis belezkeServis = new BelezkeServis();

        public void IzmeniBelezku(Belezka staraBelezka, Belezka novaBelezka)
        {
            belezkeServis.IzmeniBelezku(staraBelezka, novaBelezka);
        }

        public void SacuvajBelezku(Belezka belezka)
        {
            belezkeServis.SacuvajBelezku(belezka);
        }

        public List<Belezka> SveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            return belezkeServis.SveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        public void ObrisiBelezku(Belezka belezka)
        {
            belezkeServis.ObrisiBelezku(belezka);
        }
    }
}
