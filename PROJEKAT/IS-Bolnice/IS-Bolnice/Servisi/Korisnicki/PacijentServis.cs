﻿using System.Collections.Generic;
using System.Linq;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Servisi.Korisnici
{
    class PacijentServis
    {
        private IPacijentRepozitorijum pacijentRepo = new Injector().GetPacijentRepozitorijum();

        public Pacijent GetPacijentSaOvimJMBG(string jmbgPacijenta)
        {
            return pacijentRepo.GetPoJmbg(jmbgPacijenta);
        }

        public List<Pacijent> GetSviPacijenti()
        {
            return pacijentRepo.GetSve();
        }

        public Pacijent GetPoslednjiDodat()
        {
            List<Pacijent> pacijenti = pacijentRepo.GetSve();
            int index = pacijenti.Count() - 1;

            if (index != -1)
            {
                return pacijenti[index];
            }
            return null;
        }

        public void KreirajPacijenta(Pacijent potencijalniPacijent)
        {
            pacijentRepo.Sacuvaj(potencijalniPacijent);
        }

        public void IzmeniPacijenta(Pacijent izmenjen)
        {
            pacijentRepo.Izmeni(izmenjen);
        }

        public bool ObrisiPacijenta(string jmbgPacijenta)
        {
            if (MozeSeObrisati(jmbgPacijenta))
            {
                pacijentRepo.Obrisi(jmbgPacijenta);
                return true;
            }

            return false;
        }

        private bool MozeSeObrisati(string jmbgPacijenta)
        {
            PregledServis pregledServis = new PregledServis();
            OperacijaServis operacijaServis = new OperacijaServis();
            List<Pregled> preglediPacijenta = pregledServis.GetSviBuduciPreglediPacijenta(jmbgPacijenta);
            List<Operacija> operacijePacijenta = operacijaServis.GetSveBuduceOperacijePacijenta(jmbgPacijenta);

            return preglediPacijenta.Count == 0 && operacijePacijenta.Count == 0;
        }

        
    }
}