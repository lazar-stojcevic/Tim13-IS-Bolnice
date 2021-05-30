using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IOperacijaRepozitorijum: GenerickiRepozitorijum<Operacija>
    {
        List<Operacija> GetSveOperacijeLekara(string jmbgLekara);

        List<Operacija> GetSveBuduceOperacije();

        List<Operacija> GetSveBuduceOperacijePacijenta(string jmbgPacijenta);

        List<Operacija> GetSveBuduceOperacijeSale(string idSale);

        List<Operacija> GetSveBuduceOperacijeLekara(string jmbgLekara);
    }
}
