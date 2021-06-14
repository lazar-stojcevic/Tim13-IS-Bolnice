using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Servisi.Lekovi
{
    class OdgovorNaZahtevZaValidacijuServis
    {
        private IOdgovorNaZahtevRepozitorijum odgovorNaZahtevRepo = new Injector().GetOdgovorNaZahtevRepozitorijum();
        private IZahteviZaValidacijuRepozitorijum zahteviZaValidacijuRepo = new Injector().GetZahteviZaValidacijuRepozitorijum();

        public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
        {
            this.odgovorNaZahtevRepo.Sacuvaj(odgovor);
            zahteviZaValidacijuRepo.Obrisi(zahtev.Id);
        }

        public void ObrisiOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor)
        {
            odgovorNaZahtevRepo.Obrisi(odgovor.Id);
        }

        public List<OdgovorNaZahtevZaValidaciju> GetSve()
        {
            return odgovorNaZahtevRepo.GetSve();
        }
    }
}
