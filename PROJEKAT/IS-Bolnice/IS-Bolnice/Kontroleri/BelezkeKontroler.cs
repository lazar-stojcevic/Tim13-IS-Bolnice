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

        public void IzmeniBelezku(Beleska staraBeleska, Beleska novaBeleska)
        {
            belezkeServis.IzmeniBelezku(staraBeleska, novaBeleska);
        }

        public void SacuvajBelezku(Beleska beleska)
        {
            belezkeServis.SacuvajBelezku(beleska);
        }

        public List<Beleska> SveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            return belezkeServis.SveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        public void ObrisiBelezku(Beleska beleska)
        {
            belezkeServis.ObrisiBelezku(beleska);
        }
    }
}
