using System.Collections.Generic;
using IS_Bolnice.Servisi.Lekovi;

namespace IS_Bolnice.Kontroleri.Lekovi
{
    class SastojakKontroler
    {
        private SastojakServis sastojakServis = new SastojakServis();

        public List<Sastojak> GetSviSastojci()
        {
            return sastojakServis.GetSviSastojci();
        }

        internal void KreirajNoviSastojak(Sastojak sastojak)
        {
            sastojakServis.KreirajNoviSastojak(sastojak);
        }

        public void ObrisiSastojak(string idSastojka)
        {
            sastojakServis.ObrisiSastojak(idSastojka);
        }
    }
}
