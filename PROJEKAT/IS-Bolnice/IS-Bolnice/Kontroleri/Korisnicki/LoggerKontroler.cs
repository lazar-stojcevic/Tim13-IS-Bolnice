using IS_Bolnice.DTOs;
using IS_Bolnice.Servisi.Korisnicki;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class LoggerKontroler
    {
        private LoggerServis servis = new LoggerServis();
        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            return servis.GetKorisnika(korisnickoIme, sifra);
        }

        public void KreirajKorisnika(string korisnickoIme, string sifra, string tip)
        {
            servis.KreirajKorisnika(korisnickoIme, sifra, tip);
        }

        public void ObrisiKorisnika(string korisnickoIme)
        {
            servis.ObrisiKorisnika(korisnickoIme);
        }
    }
}
