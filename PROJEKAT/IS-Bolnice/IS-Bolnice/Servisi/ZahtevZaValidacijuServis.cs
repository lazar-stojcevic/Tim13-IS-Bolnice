using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class ZahtevZaValidacijuServis
    {
        private BazaZahtevaZaValidacijuLeka bazaZahteva = new BazaZahtevaZaValidacijuLeka();

        public void ObrisiZahtevZaValidacijuLeka(ZahtevZaValidacijuLeka zahtev)
        {
            bazaZahteva.Obrisi(zahtev.Id);
        }

        public List<ZahtevZaValidacijuLeka> GetSviZaValidacijuLeka()
        {
            return bazaZahteva.DobaviSve();
        }

        internal void KreirajZahtevZaValidaciju(ZahtevZaValidacijuLeka zahtevZaValidaciju)
        {
            bazaZahteva.Sacuvaj(zahtevZaValidaciju);
        }
    }
}
