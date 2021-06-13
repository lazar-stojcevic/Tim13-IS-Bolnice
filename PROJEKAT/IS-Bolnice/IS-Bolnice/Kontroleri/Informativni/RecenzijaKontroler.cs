using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Informativni;

namespace IS_Bolnice.Kontroleri.Informativni
{
    class RecenzijaKontroler
    {
        private RecenzijaServis recenzijaServis = new RecenzijaServis();
        public void KreirajRecenziju(Recenzija recenzija)
        {
            recenzijaServis.KreirajRecenziju(recenzija);
        }
    }
}
