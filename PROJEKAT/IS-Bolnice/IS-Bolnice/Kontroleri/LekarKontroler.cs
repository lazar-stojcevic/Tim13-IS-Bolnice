using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class LekarKontroler
    {
        private LekarServis lekarServis = new LekarServis();

        public List<Lekar> GetSviLekari()
        {
            return lekarServis.GetSviLekari();
        }

        public List<Lekar> GetSviLekariOpstePrakse()
        {
            return lekarServis.GetSviLekariOpstePrakse();
        }

        public List<Lekar> GetSviLekariSpecijalisti()
        {
            return lekarServis.GetSviLekariSpecijalisti();
        }
    }
}
