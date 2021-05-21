﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class ZahtevZaValidacijuServis
    {
        private BazaZahtevaZaValidacijuLeka bazaZahteva = new BazaZahtevaZaValidacijuLeka();

        public void ObrisiZahtevZaValidacijuLeka(ZahtevZaValidacijuLeka zahtev)
        {
            bazaZahteva.ObrisiZahtev(zahtev);
        }

        public List<ZahtevZaValidacijuLeka> GetSviZaValidacijuLeka()
        {
            return bazaZahteva.SviZahtevi();
        }
    }
}