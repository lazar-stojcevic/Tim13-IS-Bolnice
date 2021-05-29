using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface LekarRepozitorijum: GenerickiRepozitorijum<Lekar>
    { 
       List<Lekar> LekariOpstePrakse();

       List<Lekar> LekariSpecijalisti();

       List<Lekar> LekariOdredjeneOblasti(string trazenaOblast);
    }
}
