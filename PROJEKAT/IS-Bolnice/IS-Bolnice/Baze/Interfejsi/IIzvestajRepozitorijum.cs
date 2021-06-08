using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IIzvestajRepozitorijum: GenerickiRepozitorijum<Izvestaj>
    {
        List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta);

        List<Izvestaj> DobaviSveIzvestajeizPoslednjihNedeljuDana();

        List<Izvestaj> DobaviSveIzvestajeizPoslednjihMesecDana();

        List<Izvestaj> DobaviSveIzvestajeizPoslednjihGodinuDana();


    }
}
