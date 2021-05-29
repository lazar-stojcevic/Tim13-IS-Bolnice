using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IRadnoVremeRepozitorijum : GenerickiRepozitorijum<RadnoVremeLekara>
    {
        RadnoVremeLekara RadnoVremeOdredjenogLekara(string jmbg);
    }
}
