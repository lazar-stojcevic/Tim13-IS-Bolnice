using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class SastojakServis
    {
        private ISastojakRepozitorijum sastojakRepo = new SastojakFajlRepozitorijum();

        public List<Sastojak> GetSviSastojci()
        {
            return sastojakRepo.DobaviSve();
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
