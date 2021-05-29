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
        private BazaZahtevaZaValidacijuLeka bazaZahteva = new BazaZahtevaZaValidacijuLeka();

        public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
        {
            bazaOdgovora.Sacuvaj(odgovor);
            bazaZahteva.Obrisi(zahtev.Id);
        }
    }
}
