using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class IzvestajKontroler
    {
        private IzvestajServis izvestajServis = new IzvestajServis();

        public void KreirajIzvestaj(string textIzvestaja)
        {
            izvestajServis.KreirajIzvestaj(textIzvestaja);
        }
    }
}
