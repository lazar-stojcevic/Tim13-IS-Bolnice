using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi.Informativni
{
    class RecenzijaServis
    {
        private IRecenzijaRepozitorijum recRepo = new Injector().GetRecenzijaRepozitorijum();
        public void KreirajRecenziju(Recenzija recenzija)
        {
            recRepo.Sacuvaj(recenzija);
        }
    }
}
