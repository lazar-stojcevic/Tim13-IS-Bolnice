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
            List<Lekar> listaSvihLekara = new List<Lekar>();
            foreach (Lekar lekar in lekarRepo.DobaviSve())
            {
                if (!lekar.JelLekarOpstePrakse())
                {
                    listaSvihLekara.Add(lekar);
                }
            }

            return listaSvihLekara;
        }

        public List<Lekar> GetSviLekariOpstePrakse()
        {
            List<Lekar> listaSvihLekaraOpstePrakse = new List<Lekar>();
            foreach (Lekar lekar in lekarRepo.DobaviSve())
            {
                if (lekar.JelLekarOpstePrakse())
                {
                    listaSvihLekaraOpstePrakse.Add(lekar);
                }
            }

            return listaSvihLekaraOpstePrakse;
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
