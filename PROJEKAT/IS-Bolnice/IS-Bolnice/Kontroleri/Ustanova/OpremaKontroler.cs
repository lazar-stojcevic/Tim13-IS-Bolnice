using System.Collections.Generic;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Kontroleri.Ustanova
{
    class OpremaKontroler
    {
        OpremaServis servis = new OpremaServis();
        internal void KreirajNoviPredmet(Predmet noviPredmet)
        {
            servis.KreirajNoviPredmet(noviPredmet);
        }

        public Predmet GetPoId(string idOpreme)
        {
            return servis.GetPoId(idOpreme);
        }

        public void IzmeniPredmet(Predmet izmenjenPredmet)
        {
            servis.IzmeniPredmet(izmenjenPredmet);
        }

        public bool ObrisiPredmet(string idPredmeta)
        {
            return servis.ObrisiPredmet(idPredmeta);
        }

        public List<Predmet> GetSvaOprema()
        {
            return servis.GetSvaOprema();
        }
    }
}
