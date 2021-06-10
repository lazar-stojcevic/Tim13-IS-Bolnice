using IS_Bolnice.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Kontroleri
{
    class UpravnikKontroler
    {
        UpravnikServis upravnikServis = new UpravnikServis();

        public void Izmeni(Upravnik upravnik) {
            upravnikServis.Izmeni(upravnik);
        }

        public Upravnik GetByJmbg(string jmbg)
        {
            return upravnikServis.GetByJmbg(jmbg);
        }
    }
}
