using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class OdgovorNaZahtevZaValidacijuServis
    {
        private IOdgovorNaZahtevRepozitorijum odgovorNaZahtevRepo = new OdgovorNaZahtevFajlRepozitorijum();
        private IZahteviZaValidacijuRepozitorijum zahteviZaValidacijuRepo = new ZahteviZaValidacijuFajlRepozitorijum();

        public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
        {
            this.odgovorNaZahtevRepo.Sacuvaj(odgovor);
            zahteviZaValidacijuRepo.Obrisi(zahtev.Id);
        }
    }
}
