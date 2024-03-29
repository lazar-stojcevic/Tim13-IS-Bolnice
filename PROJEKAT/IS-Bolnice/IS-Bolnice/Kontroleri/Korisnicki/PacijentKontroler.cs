﻿using System.Collections.Generic;
using IS_Bolnice.Servisi;
using IS_Bolnice.Servisi.Korisnici;
using IS_Bolnice.Servisi.Korisnicki;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class PacijentKontroler
    {
        private PacijentServis pacijentServis = new PacijentServis();
        private LoggerServis loggerServis = new LoggerServis();

        public Pacijent GetPacijentSaOvimJMBG(string jmbgPacijenta)
        {
            return pacijentServis.GetPacijentSaOvimJMBG(jmbgPacijenta);
        }

        public List<Pacijent> GetSviPacijenti()
        {
            return pacijentServis.GetSviPacijenti();
        }

        public Pacijent GetPoslednjiDodat()
        {
            return pacijentServis.GetPoslednjiDodat();
        }

        public void KreirajPacijenta(Pacijent potencijalniPacijent)
        {
            pacijentServis.KreirajPacijenta(potencijalniPacijent);
            loggerServis.KreirajKorisnika(potencijalniPacijent.KorisnickoIme, potencijalniPacijent.Sifra, "P");
        }

        public void IzmeniPacijenta(Pacijent izmenjen, Pacijent stari)
        {
            pacijentServis.IzmeniPacijenta(izmenjen);
            loggerServis.ObrisiKorisnika(stari.KorisnickoIme);
            loggerServis.KreirajKorisnika(izmenjen.KorisnickoIme, izmenjen.Sifra, "P");
        }

        public bool ObrisiPacijenta(string jmbgPacijenta)
        {
            Pacijent pacijent = GetPacijentSaOvimJMBG(jmbgPacijenta);
            bool uspesno = pacijentServis.ObrisiPacijenta(jmbgPacijenta);
            if (uspesno)
            {
                loggerServis.ObrisiKorisnika(pacijent.KorisnickoIme);
            }
            return uspesno;
        }
    }
}
