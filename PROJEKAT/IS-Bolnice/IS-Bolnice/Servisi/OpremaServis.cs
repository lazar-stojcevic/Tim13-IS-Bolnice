using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class OpremaServis
    {
        IOpremaRepozitorijum opremaRepo = new OpremaFajlRepozitorijum();
        internal void KreirajNoviPredmet(Predmet noviPredmet)
        {
            opremaRepo.Sacuvaj(noviPredmet);
        }
    }
}
