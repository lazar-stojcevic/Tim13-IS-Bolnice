using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class PacijentKontroler
    {
        private PacijentServis pacijentServis = new PacijentServis();

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
        }

        public void IzmeniPacijenta(Pacijent izmenjen, Pacijent pocetni)
        {
            pacijentServis.IzmeniPacijenta(izmenjen, pocetni);
        }

        public void ObrisiPacijenta(Pacijent pacijent)
        {
            pacijentServis.ObrisiPacijenta(pacijent);
        }
    }
}
