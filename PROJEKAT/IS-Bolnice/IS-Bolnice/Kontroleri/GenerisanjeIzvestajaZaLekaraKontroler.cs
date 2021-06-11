using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.GenerisanjeIzvestaja;

namespace IS_Bolnice.Kontroleri
{
    class GenerisanjeIzvestajaZaLekaraKontroler
    {
        private GenerisanjeIzvestajaZaLekara generisanjeIzvestaja = new GenerisanjePDFIzvestajaZaLekaraServis();

        public bool GenerisiIzvestajZauzetostiLekara(Lekar lekar, VremenskiInterval intervalIzvestaja)
        {
            return generisanjeIzvestaja.GenerisanjeIzvestajaZauzetostiLekara(lekar, intervalIzvestaja);
        }
    }
}
