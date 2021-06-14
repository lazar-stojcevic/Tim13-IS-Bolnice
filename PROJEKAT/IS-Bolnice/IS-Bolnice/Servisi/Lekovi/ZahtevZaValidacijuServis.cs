using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Lekovi
{
    class ZahtevZaValidacijuServis
    {
        private IZahteviZaValidacijuRepozitorijum zahteviZaValidacijuRepo = new Injector().GetZahteviZaValidacijuRepozitorijum();

        public void ObrisiZahtevZaValidacijuLeka(ZahtevZaValidacijuLeka zahtev)
        {
            zahteviZaValidacijuRepo.Obrisi(zahtev.Id);
        }

        public List<ZahtevZaValidacijuLeka> GetSviZaValidacijuLeka()
        {
            return zahteviZaValidacijuRepo.GetSve();
        }

        internal void KreirajZahtevZaValidaciju(ZahtevZaValidacijuLeka zahtevZaValidaciju)
        {
            zahteviZaValidacijuRepo.Sacuvaj(zahtevZaValidaciju);
        }
    }
}
