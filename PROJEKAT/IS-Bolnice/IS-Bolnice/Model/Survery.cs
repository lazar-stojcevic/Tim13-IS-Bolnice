using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    //Anketa
    class Survery : Entitet
    {
        public Survery() { }
        public Pacijent Patient { get; set; }
        public Lekar Doctor { get; set; }
        public Bolnica Hospital { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime TimeLimit { get; set; }
    }
}
