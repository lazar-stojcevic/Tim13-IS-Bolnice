using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IIzvestajRepozitorijum: GenerickiRepozitorijum<Izvestaj>
    {
        List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta);

        List<Izvestaj> GetSviIzvestajiIzPoslednjihNedeljuDana();

        List<Izvestaj> GetSviIzvestajiIzPoslednjihMesecDana();

        List<Izvestaj> GetSviIzvestajiIzPoslednjihGodinuDana();
    }
}
