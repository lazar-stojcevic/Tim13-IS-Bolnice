using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IZahteviZaValidacijuRepozitorijum : GenerickiRepozitorijum<ZahtevZaValidacijuLeka>
    {
        List<ZahtevZaValidacijuLeka> GetZahteviZaValidacijuZaLekara(string idLekara);
    }
}
