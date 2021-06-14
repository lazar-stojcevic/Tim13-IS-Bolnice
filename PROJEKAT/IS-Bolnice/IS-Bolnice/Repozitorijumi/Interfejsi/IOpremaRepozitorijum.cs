using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IOpremaRepozitorijum: GenerickiRepozitorijum<Predmet>
    {
        //Ovde se definisu specijalne GET metode koje vam trebaju, tipa da se dobavi nesto po tipu ili slicno, ovo vazi za sve ove
    }

}
