using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
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

        public void ObrisiPacijenta(string jmbgPacijenta)
        {
            pacijentRepo.Obrisi(jmbgPacijenta);
        }

        
    }
}