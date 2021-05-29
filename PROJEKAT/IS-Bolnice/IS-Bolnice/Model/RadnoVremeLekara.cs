﻿using System;
using System.Collections.Generic;

namespace IS_Bolnice.Model
{
    public class RadnoVremeLekara : Entitet
    {
        public VremenskiInterval StandardnoRadnoVreme { get; set; }
        public List<VremenskiInterval> VanrednaRadnaVremena { get; set; }
        public List<DateTime> SlobodniDani { get; set; }
        public List<DayOfWeek> SlobodniDaniUNedelji { get; set; }

        public int PreostaliSlobodniDaniUGodini { get; set; }

        public RadnoVremeLekara() : base()
        {
            VanrednaRadnaVremena = new List<VremenskiInterval>();
            SlobodniDani = new List<DateTime>();
            SlobodniDaniUNedelji = new List<DayOfWeek>();
            PreostaliSlobodniDaniUGodini = 30;
        }

        public bool PripadaLekaru(string jmbg)
        {
            return this.Id.Equals(jmbg);
        }
    }
}
