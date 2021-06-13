using System.Collections.Generic;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Korisnici
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
