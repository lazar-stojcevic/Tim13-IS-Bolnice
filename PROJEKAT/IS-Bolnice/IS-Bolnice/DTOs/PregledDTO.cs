using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.DTOs
{
    class PregledDTO
    {
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public DateTime VremePocetkaPregleda { get; set; }
        public DateTime VremeKrajaPregleda { get; set; }
        public Soba Soba { get; set; }
        public Boolean Hitna { get; set; }
        public int TrajanjePregledaUMinutima { get; set; }
    }
}
