using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    //Anketa
    class Anketa : Entitet
    {
        public Anketa() { }
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public Bolnica Bolnica { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; }
        public DateTime Trajanje { get; set; }
        public int KojaAnketa { get; set; } // 0 za lekara 1 za bolnicu
    }
}
