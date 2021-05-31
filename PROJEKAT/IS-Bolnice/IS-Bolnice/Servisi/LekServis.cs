using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class LekServis
    {
        private ILekRepozitorijum lekRepo = new LekFajlRepozitorijum();

        public List<Lek> GetSviLekovi()
        {
            return lekRepo.DobaviSve();
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

        public Lek DobaviLekPoId(string idLeka)
        {
            return lekRepo.DobaviPoId(idLeka);
        }
    }
}
