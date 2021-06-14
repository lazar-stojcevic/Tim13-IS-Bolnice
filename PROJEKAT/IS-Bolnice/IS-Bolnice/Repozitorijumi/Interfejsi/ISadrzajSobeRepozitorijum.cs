using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface ISadrzajSobeRepozitorijum: GenerickiRepozitorijum<SadrzajSobe>
    {
        List<SadrzajSobe> GetSadrzajSobe(string idSobe);

        List<SadrzajSobe> GetDinamickiSadrzajSobe(string idSobe);
    }
}
