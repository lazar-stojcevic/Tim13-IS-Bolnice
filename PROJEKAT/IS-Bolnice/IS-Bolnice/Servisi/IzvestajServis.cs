﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class IzvestajServis
    {
        private BazaIzvestaja bazaIzvestaja = new BazaIzvestaja();

        public void KreirajIzvestaj(Izvestaj izvestaj)
        {
            bazaIzvestaja.Sacuvaj(izvestaj);
        }

        public List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta)
        {
            List<Izvestaj> izvestajiPacijenta = new List<Izvestaj>();

            foreach (Izvestaj izvestaj in bazaIzvestaja.DobaviSve())
            {
                if (izvestaj.Pacijent.Jmbg == jmbgPacijenta)
                {
                    izvestajiPacijenta.Add(izvestaj);
                }
            }

            return izvestajiPacijenta;
        }
    }
}
