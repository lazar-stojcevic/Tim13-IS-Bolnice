using IS_Bolnice.Servisi.GenerisanjeIzvestaja;

namespace IS_Bolnice.Kontroleri.GenerisanjeIzvestaja
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
