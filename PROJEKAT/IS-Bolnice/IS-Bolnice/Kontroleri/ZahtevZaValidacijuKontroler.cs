using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
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
