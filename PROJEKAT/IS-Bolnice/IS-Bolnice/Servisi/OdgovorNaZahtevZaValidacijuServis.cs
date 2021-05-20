using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class OdgovorNaZahtevZaValidacijuServis
    {
        private BazaOdgovoraNaZahteveZaValidacijuLekova bazaOdgovora = new BazaOdgovoraNaZahteveZaValidacijuLekova();

        public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
        {
            bazaOdgovora.KreirajOdgovorNaZahtev(odgovor, zahtev);
        }
    }
}
