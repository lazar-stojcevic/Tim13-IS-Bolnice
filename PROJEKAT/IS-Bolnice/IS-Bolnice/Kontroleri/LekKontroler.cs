using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class LekKontroler
    {
        private LekServis lekServis = new LekServis();

        public List<Lek> GetSviLekovi()
        {
            return lekServis.GetSviLekovi();
        }

        public void ObrisiLek(Lek lek)
        {
            lekServis.ObrisiLek(lek);
        }

        public void KreirajLek(Lek lek)
        {
            lekServis.KreirajLek(lek);
        }
    }
}
