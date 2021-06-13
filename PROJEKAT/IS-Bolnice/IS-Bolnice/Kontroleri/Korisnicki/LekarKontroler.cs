using System.Collections.Generic;
using IS_Bolnice.Servisi.Korisnici;

namespace IS_Bolnice.Kontroleri.Korisnicki
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

        public Lekar GetLekar(string jmbgLekara)
        {
            return lekarServis.GetLekar(jmbgLekara);
        }
    }
}
