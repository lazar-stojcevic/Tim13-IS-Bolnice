using IS_Bolnice.Model;
using System.Collections.Generic;


namespace IS_Bolnice.Baze.Interfejsi
{
    interface IAnketaRepozitorijum : GenerickiRepozitorijum<Anketa>
    {
        List<Anketa> GetSveAnketeBolnice();
    }
}
