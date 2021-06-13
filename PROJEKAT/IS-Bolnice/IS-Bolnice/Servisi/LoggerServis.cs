using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class LoggerServis
    {
        private ILoggerRepozitorijum repo = new Injector().GetLoggerRepozitorijum();
        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            Model.Logger logger = repo.GetPoId(korisnickoIme);
            if (logger == null) return null;
            return logger.LogInServis.GetKorisnika(korisnickoIme, sifra);
        }

        public void KreirajKorisnika(string korisnickoIme, string sifra, string tip)
        {
            Logger logger = new Logger(korisnickoIme, sifra, tip);
            repo.Sacuvaj(logger);
        }

        public void ObrisiKorisnika(string korisnickoIme)
        {
            repo.Obrisi(korisnickoIme);
        }
    }
}
