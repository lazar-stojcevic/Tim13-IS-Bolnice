using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IBolnicaRepozitorijum: GenerickiRepozitorijum<Bolnica>
    {
        Soba GetSobaById(string idSobe);

        Soba GetMagacin();

        List<Soba> GetSobe();

        Bolnica GetBolnica();

        List<Soba> SveOperacioneSaleOveBolnice();
    }
}
