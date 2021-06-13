using System.Collections.Generic;
using IS_Bolnice.Servisi.Lekovi;

namespace IS_Bolnice.Kontroleri.Lekovi
{
    class ZahtevZaValidacijuKontroler
    {
        private ZahtevZaValidacijuServis servis = new ZahtevZaValidacijuServis();

        public void ObrisiZahtev(ZahtevZaValidacijuLeka zahtev)
        {
            servis.ObrisiZahtevZaValidacijuLeka(zahtev);
        }

        public List<ZahtevZaValidacijuLeka> GetSviZaValidacijuLeka()
        {
            return servis.GetSviZaValidacijuLeka();
        }

        public void KreirajZahtevZaValidaciju(ZahtevZaValidacijuLeka zahtevZaValidaciju)
        {
            servis.KreirajZahtevZaValidaciju(zahtevZaValidaciju);
        }
    }
}
