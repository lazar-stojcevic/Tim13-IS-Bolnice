using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class LekarServis
    {
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();
        public List<Lekar> GetSviLekariSpecijalisti()
        {
            return lekarRepo.GetSviLekariSpecijalisti();
        }

        public List<Lekar> GetSviLekariOpstePrakse()
        {
            return lekarRepo.GetSviLekariOpstePrakse();
        }

        public List<Lekar> GetSviLekari()
        {
            return lekarRepo.GetSve();
        }

        public Lekar GetLekar(string jmbgLekara)
        {
            return lekarRepo.GetPoId(jmbgLekara);
        }

    }
}
