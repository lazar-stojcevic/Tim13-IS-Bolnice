using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IRenovacijaRepozitorijum: GenerickiRepozitorijum<Renovacija>
    {
        List<Renovacija> SveRenovacijeJedneSobe(Soba soba);
    }
}
