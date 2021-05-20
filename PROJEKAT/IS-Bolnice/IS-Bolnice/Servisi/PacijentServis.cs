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

    }
}
