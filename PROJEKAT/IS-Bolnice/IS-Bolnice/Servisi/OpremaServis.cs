﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class OpremaServis
    {
        BazaOpreme bazaOpreme = new BazaOpreme();
        internal void KreirajNoviPredmet(Predmet noviPredmet)
        {
            bazaOpreme.Sacuvaj(noviPredmet);
        }
    }
}
