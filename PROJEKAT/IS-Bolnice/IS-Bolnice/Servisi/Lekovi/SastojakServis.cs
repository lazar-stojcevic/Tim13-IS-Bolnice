using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Lekovi
{
    class SastojakServis
    {
        private ISastojakRepozitorijum sastojakRepo = new Injector().GetSastojakRepozitorijum();

        public List<Sastojak> GetSviSastojci()
        {
            return sastojakRepo.GetSve();
        }

        internal void KreirajNoviSastojak(Sastojak noviSastojak)
        {
            sastojakRepo.Sacuvaj(noviSastojak);
        }

        public void ObrisiSastojak(string isSastojak)
        {
            sastojakRepo.Obrisi(isSastojak);
        }
    }
}
