using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class PacijentServis
    {
        private IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();

        public Pacijent GetPacijentSaOvimJMBG(string jmbgPacijenta)
        {
            foreach (Pacijent pacijent in pacijentRepo.DobaviSve())
            {
                if (pacijent.Jmbg.Equals(jmbgPacijenta))
                {
                    return pacijent;
                }
            }
            return null;
        }

        public List<Pacijent> GetSviPacijenti()
        {
            return pacijentRepo.DobaviSve();
        }

        public Pacijent GetPoslednjiDodat()
        {
            List<Pacijent> pacijenti = pacijentRepo.DobaviSve();
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