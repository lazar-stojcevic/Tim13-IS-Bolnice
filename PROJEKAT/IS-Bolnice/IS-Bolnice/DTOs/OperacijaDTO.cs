using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.DTOs
{
    class OperacijaDTO
    {
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public DateTime VremePocetkaOperacije { get; set; }
        public DateTime VremeKrajaOperacije { get; set; }
        public Soba Soba { get; set; }
        public Boolean Hitna { get; set; }
        public int TrajanjeOperacijeUMinutima { get; set; }
    }
}
