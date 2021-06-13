using System.Collections.Generic;
using IS_Bolnice.Servisi.Lekovi;

namespace IS_Bolnice.Kontroleri.Lekovi
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

        public List<OdgovorNaZahtevZaValidaciju> GetSve()
        {
            return odgovorNaZahtevServis.GetSve();
        }
    }
}
