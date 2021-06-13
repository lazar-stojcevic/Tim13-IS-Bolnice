using System.Collections.Generic;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Informativni;

namespace IS_Bolnice.Kontroleri.Informativni
{
    class BelezkeKontroler
    {
        private BelezkeServis belezkeServis = new BelezkeServis();

        public void IzmeniBelezku(Beleska izmenjenaBeleska)
        {
            belezkeServis.IzmeniBelezku(izmenjenaBeleska);
        }

        public void SacuvajBelezku(Beleska beleska)
        {
            belezkeServis.SacuvajBelezku(beleska);
        }

        public List<Beleska> GetSveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            return belezkeServis.GetSveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        public void ObrisiBelezku(string idBeleske)
        {
            belezkeServis.ObrisiBelezku(idBeleske);
        }
    }
}
