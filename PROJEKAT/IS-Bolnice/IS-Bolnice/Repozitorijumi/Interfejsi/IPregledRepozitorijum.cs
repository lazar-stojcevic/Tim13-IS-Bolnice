using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IPregledRepozitorijum: GenerickiRepozitorijum<Pregled>
    {
        List<Pregled> GetSviPreglediLekara(string jmbgLekara);

        List<Pregled> GetSviBuduciPregledi();

        List<Pregled> GetSviBuduciPreglediLekara(string jmbgLekara);

        List<Pregled> GetSviBuduciPreglediSobe(string idSobe);

        List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta);
    }
}
