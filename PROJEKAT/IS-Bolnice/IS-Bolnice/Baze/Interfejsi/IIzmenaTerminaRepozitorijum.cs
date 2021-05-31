using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IIzmenaTerminaRepozitorijum : GenerickiRepozitorijum<IzmenaTermina>
    {
        void OdblokirajPacijenta(Pacijent pacijent);
        bool DaLiJeJmbgJednak(IzmenaTermina izmenaTermina, Pacijent pacijent);
        List<IzmenaTermina> GetIzmenePacijenta(Pacijent pacijent);
    }
}
