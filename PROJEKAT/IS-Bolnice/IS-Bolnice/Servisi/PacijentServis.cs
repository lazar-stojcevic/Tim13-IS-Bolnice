using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class PacijentServis
    {
        private BazaPacijenata bazaPacijenata = new BazaPacijenata();

        public Pacijent GetPacijentSaOvimJMBG(string jmbgPacijenta)
        {
            foreach (Pacijent pacijent in bazaPacijenata.SviPacijenti())
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
            return bazaPacijenata.SviPacijenti();
        }

        public void KreirajPacijenta(Pacijent potencijalniPacijent)
        {
            bazaPacijenata.KreirajPacijenta(potencijalniPacijent);
        }

        public void IzmeniPacijenta(Pacijent izmenjen, Pacijent pocetni)
        {
            bazaPacijenata.IzmeniPacijenta(izmenjen, pocetni);
        }

        public bool JedinstvenJmbgPacijenta(string jmbg)
        {
            foreach (Pacijent p in bazaPacijenata.SviPacijenti())
            {
                if (p.Jmbg.Equals(jmbg))
                {
                    return false;
                }
            }

            return true;
        }

        public bool JedinstvenoKorisnickoIme(string korisnickoIme)
        {
            foreach (Pacijent p in bazaPacijenata.SviPacijenti())
            {
                if (p.KorisnickoIme.Equals(korisnickoIme))
                {
                    return false;
                }
            }

            return true;
        }
    }
}