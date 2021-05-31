﻿using IS_Bolnice.Model;
using IS_Bolnice.Servisi;
using System.Collections.Generic;

namespace IS_Bolnice.Kontroleri
{
    class AnketaKontroler
    {
        AnketaServis anketaServis = new AnketaServis();

        public List<Pregled> GetSviPreglediZaAnketu(string patientJmbg)
        {
            return anketaServis.GetSviPreglediZaAnketu(patientJmbg);
        }

        public bool DaLiJeVremeZaAnketuBolnice(string jmbgPacijenta)
        {
            return anketaServis.DaLiJeVremeZaAnketuBolnice(jmbgPacijenta);
        }

        public void SacuvajAnketu(Anketa anketa)
        {
            anketaServis.SacuvajAnketu(anketa);
        }
    }
}