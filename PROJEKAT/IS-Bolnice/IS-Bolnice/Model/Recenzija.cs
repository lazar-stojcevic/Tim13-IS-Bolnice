using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    class Recenzija:Entitet
    {
        public string Opis { get; set; }
        public int Ocena { get; set; }

        public Recenzija():base(Guid.NewGuid().ToString())
        {

        }

        public override string ToString()
        {
            return Id + "#" + Ocena + "#" + Opis;
        }

    }
}
