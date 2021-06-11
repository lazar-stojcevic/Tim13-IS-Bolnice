using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi.GenerisanjeIzvestaja;

namespace IS_Bolnice.Kontroleri
{
    class GenerisanjeIzvestajaZaPacijentaKontroler
    {
        private GenerisanjeIzvestajaZaPacijenta generisanjeIzvestaja = new GenerisanjePDFIzvestajaZaPacijentaServis();

        public bool GenerisiIzvestajBuducihOperacijaPacijenta(string jmbgPacijenta)
        {
            return generisanjeIzvestaja.GenerisanjeIzvestajaBuducihOperacijaPacijenta(jmbgPacijenta);
        }
    }
}
