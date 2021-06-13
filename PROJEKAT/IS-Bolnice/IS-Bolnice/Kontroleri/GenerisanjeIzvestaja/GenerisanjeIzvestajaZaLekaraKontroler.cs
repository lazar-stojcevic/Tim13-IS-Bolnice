using IS_Bolnice.Model;
using IS_Bolnice.Servisi.GenerisanjeIzvestaja;

namespace IS_Bolnice.Kontroleri.GenerisanjeIzvestaja
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
