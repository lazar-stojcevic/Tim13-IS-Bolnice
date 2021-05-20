using System;
using System.Collections.Generic;

namespace IS_Bolnice.Model
{
    public class RadnoVremeLekara
    {
        public string Id { get; set; }
        public VremenskiInterval StandardnoRadnoVreme { get; set; }
        public List<VremenskiInterval> VanrednaRadnaVremena { get; set; }
        public List<DateTime> SlobodniDani { get; set; }
        public List<DayOfWeek> SlobodniDaniUNedelji { get; set; }

        public RadnoVremeLekara()
        {
            VanrednaRadnaVremena = new List<VremenskiInterval>();
            SlobodniDani = new List<DateTime>();
            SlobodniDaniUNedelji = new List<DayOfWeek>();
        }

        public bool PripadaLekaru(string jmbg)
        {
            return this.Id.Equals(jmbg);
        }
    }
}
