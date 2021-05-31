using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class OdgovoriNaZahtevZaValidacijeKontroler
    {
        private OdgovorNaZahtevZaValidacijuServis odgovorNaZahtevServis = new OdgovorNaZahtevZaValidacijuServis();

        public void KreirajOdgovorNaZahtevZaValidaciju(OdgovorNaZahtevZaValidaciju odgovor,
            ZahtevZaValidacijuLeka zahtev)
        {
            odgovor.Lek = zahtev.Lek;
            odgovorNaZahtevServis.KreirajOdgovorNaZahtev(odgovor, zahtev);
        }

        public void ObrisiOdgovorNaZahtevZaValidaciju(OdgovorNaZahtevZaValidaciju odgovor)
        {
            odgovorNaZahtevServis.ObrisiOdgovorNaZahtev(odgovor);
        }

        public List<OdgovorNaZahtevZaValidaciju> DobaviSve()
        {
            return odgovorNaZahtevServis.DobaviSve();
        }
    }
}
