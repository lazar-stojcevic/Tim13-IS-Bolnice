using IS_Bolnice.Model;
using System.Collections.Generic;


namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IAnketaRepozitorijum : GenerickiRepozitorijum<Anketa>
    {
        List<Anketa> GetSveAnketeBolnice();
        List<Anketa> GetSveAnketeLekara();
    }
}
