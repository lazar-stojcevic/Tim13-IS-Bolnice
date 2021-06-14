using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi.Korisnicki
{
    class OblastLekaraServis
    {
        private IOblastLekaraRepozitorijum oblastLekaraRepo = new Injector().GetOblastLekaraRepozitorijum();

        public List<OblastLekara> GetSveOblastiLekara()
        {
            return oblastLekaraRepo.GetSve();
        }
    }
}
