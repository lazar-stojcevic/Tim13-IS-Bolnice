using IS_Bolnice.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Kontroleri
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

        public void ObrisiPredmet(string idPredmeta)
        {
            servis.ObrisiPredmet(idPredmeta);
        }

        public List<Predmet> GetSvaOprema()
        {
            return servis.GetSvaOprema();
        }
    }
}
