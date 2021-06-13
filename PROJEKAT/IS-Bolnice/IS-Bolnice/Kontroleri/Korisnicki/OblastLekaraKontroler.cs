using System.Collections.Generic;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Korisnicki;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class OblastLekaraKontroler
    {
        private OblastLekaraServis oblastLekaraServis = new OblastLekaraServis();

        public List<OblastLekara> GetSveOblastiLekara()
        {
            return oblastLekaraServis.GetSveOblastiLekara();
        }
    }
}
