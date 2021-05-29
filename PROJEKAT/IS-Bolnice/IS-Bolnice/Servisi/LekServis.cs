using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class LekServis
    {
        private BazaLekova bazaLekova = new BazaLekova();

        public List<Lek> GetSviLekovi()
        {
            return bazaLekova.DobaviSve();
        }

        public void ObrisiLek(Lek lek)
        {
            bazaLekova.Obrisi(lek.Id);
        }

        public void KreirajLek(Lek lek)
        {
            bazaLekova.Sacuvaj(lek);
        }
    }
}
