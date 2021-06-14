using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
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
