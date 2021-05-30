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
    }
}
