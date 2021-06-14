using System;

namespace IS_Bolnice.Model
{
    class Anketa : Recenzija
    {
        public Anketa() { }
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public Bolnica Bolnica { get; set; }
        public DateTime Trajanje { get; set; }
        public int KojaAnketa { get; set; } // 0 za lekara 1 za bolnicu

    }
}
