﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
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
