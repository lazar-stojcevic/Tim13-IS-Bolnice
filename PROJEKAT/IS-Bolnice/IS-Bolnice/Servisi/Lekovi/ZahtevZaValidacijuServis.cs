using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;

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
