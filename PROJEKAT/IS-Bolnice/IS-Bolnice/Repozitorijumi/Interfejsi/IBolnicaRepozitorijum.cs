using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IBolnicaRepozitorijum: GenerickiRepozitorijum<Bolnica>
    {
        Soba GetSobaById(string idSobe);

        Soba GetMagacin();

        List<Soba> GetSobe();

        Bolnica GetBolnica();

        List<Soba> GetSveOperacioneSale();

        List<Soba> GetSveSobeZaPregled();

        List<Soba> GetSveSobeZaHospitalizaciju();
    }
}
