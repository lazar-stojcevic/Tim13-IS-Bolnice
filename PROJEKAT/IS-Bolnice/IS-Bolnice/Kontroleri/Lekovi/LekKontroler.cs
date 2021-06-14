using System.Collections.Generic;
using IS_Bolnice.Servisi.Lekovi;

namespace IS_Bolnice.Kontroleri.Lekovi
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

        public void IzmeniLek(Lek lek)
        {
            lekServis.IzmeniLek(lek);
        }

        public Lek GetLekPoId(string idLeka)
        {
            return lekServis.GetLekPoId(idLeka);
        }
    }
}
