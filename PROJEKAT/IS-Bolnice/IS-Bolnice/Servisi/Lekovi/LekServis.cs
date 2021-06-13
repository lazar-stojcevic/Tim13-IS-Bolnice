using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Lekovi
{
    class LekServis
    {
        private ILekRepozitorijum lekRepo = new Injector().GetLekRepozitorijum();

        public List<Lek> GetSviLekovi()
        {
            return lekRepo.GetSve();
        }

        public void ObrisiLek(Lek lek)
        {
            lekRepo.Obrisi(lek.Id);
        }

        public void KreirajLek(Lek lek)
        {
            lekRepo.Sacuvaj(lek);
        }

        public void IzmeniLek(Lek lek)
        {
            lekRepo.Izmeni(lek);
        }

        public Lek GetLekPoId(string idLeka)
        {
            return lekRepo.GetPoId(idLeka);
        }
    }
}
