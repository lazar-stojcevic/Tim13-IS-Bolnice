using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IHospitalizacijaRepozitorijum:GenerickiRepozitorijum<Hospitalizacija>
    {
        List<Hospitalizacija> GetSveHospitalizacijeZaSobu(string sobaID);

        bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija);
    }
}
