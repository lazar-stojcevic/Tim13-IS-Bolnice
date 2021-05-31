using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
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
