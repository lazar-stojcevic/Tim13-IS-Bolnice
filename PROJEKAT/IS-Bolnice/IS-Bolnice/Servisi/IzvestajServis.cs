using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class IzvestajServis
    {
        private BazaIzvestaja bazaIzvestaja = new BazaIzvestaja();

        public void KreirajIzvestaj(string textIzvestaja)
        {
            bazaIzvestaja.KreirajIzvestaj(textIzvestaja);
        }
    }
}
