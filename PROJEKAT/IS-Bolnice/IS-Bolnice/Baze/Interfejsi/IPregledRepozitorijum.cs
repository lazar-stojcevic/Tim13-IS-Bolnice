using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IPregledRepozitorijum: GenerickiRepozitorijum<Pregled>
    {
        List<Pregled> SviBuduciPreglediKojeLekarIma(string jmbgLekara);
        List<Pregled> SviBuduciPregledi();
    }
}
