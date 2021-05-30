using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface ILekarRepozitorijum: GenerickiRepozitorijum<Lekar>
    { 
       List<Lekar> GetSviLekariOpstePrakse();

       List<Lekar> GetSviLekariSpecijalisti();

       List<Lekar> LekariOdredjeneOblasti(string trazenaOblast);
    }
}
