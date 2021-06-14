using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IObavestenjaRepozitorijum : GenerickiRepozitorijum<Obavestenje>
    {
        List<Obavestenje> SvaObavestenjaLekara();
        List<Obavestenje> SvaObavestenjaUpravnika();
        List<Obavestenje> SvaObavestenjaSekretara();
        List<Obavestenje> SvaObavestenjaPacijenata();
        List<Obavestenje> SvaObavestenjaPacijenta(Pacijent pacijent);
    }
}
