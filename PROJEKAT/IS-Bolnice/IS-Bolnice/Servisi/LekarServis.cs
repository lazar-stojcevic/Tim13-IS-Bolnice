using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class LekarServis
    {
        private ILekarRepozitorijum lekarRepo = new LekarFajlRepozitorijum();
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
            return lekarRepo.DobaviSve();
        }

        public Lekar GetLekar(string jmbgLekara)
        {
            return lekarRepo.DobaviPoId(jmbgLekara);
        }
    }
}
