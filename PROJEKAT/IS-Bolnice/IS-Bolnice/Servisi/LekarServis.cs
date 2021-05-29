using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class LekarServis
    {
        private BazaLekara bazaLekara = new BazaLekara();
        public List<Lekar> GetSviLekariSpecijalisti()
        {
            List<Lekar> listaSvihLekara = new List<Lekar>();
            foreach (Lekar lekar in bazaLekara.DobaviSve())
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
            foreach (Lekar lekar in bazaLekara.DobaviSve())
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
            return bazaLekara.DobaviSve();
        }

        public Lekar GetLekar(string jmbgLekara)
        {
            return bazaLekara.DobaviPoId(jmbgLekara);
        }
    }
}
