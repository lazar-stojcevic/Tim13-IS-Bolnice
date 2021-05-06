using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    public class OblastLekara
    {
        public static readonly string oznakaOpstePrakse = "opsta praksa";
        public string Naziv { get; set; }

        public OblastLekara(string oblast)
        {
            Naziv = oblast;
        }

        public bool Jednako(OblastLekara prosledjenaOblast)
        {
            return prosledjenaOblast.Naziv.Equals(Naziv);
        }
    }
}
