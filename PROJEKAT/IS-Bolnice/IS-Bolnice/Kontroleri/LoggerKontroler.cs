using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.DTOs;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
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
