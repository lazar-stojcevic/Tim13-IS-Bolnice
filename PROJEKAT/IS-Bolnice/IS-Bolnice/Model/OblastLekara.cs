using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    public class OblastLekara : Entitet
    {
        public static readonly string oznakaOpstePrakse = "opsta praksa";
        public string Naziv { get; set; }

        public OblastLekara() : base(Guid.NewGuid().ToString())
        {

        }
        public OblastLekara(string oblast) : base(Guid.NewGuid().ToString())
        {
            Naziv = oblast;
        }
        public OblastLekara(string id, string oblast) : base(id)
        {
            Naziv = oblast;
        }

        public bool Jednako(OblastLekara prosledjenaOblast)
        {
            return prosledjenaOblast.Naziv.Equals(Naziv);
        }

        public bool JelOpstaPraksa()
        {
            return this.Naziv.Equals(oznakaOpstePrakse);
        }
    }
}
