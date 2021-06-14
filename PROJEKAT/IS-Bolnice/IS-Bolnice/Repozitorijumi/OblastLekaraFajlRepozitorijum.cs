using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;

namespace IS_Bolnice.Repozitorijumi
{
    class OblastLekaraFajlRepozitorijum : GenerickiFajlRepozitorijum<OblastLekara>, IOblastLekaraRepozitorijum
    {
        public OblastLekaraFajlRepozitorijum() : base(@"..\..\Datoteke\oblastiLekara.txt")
        {
        }

        public override OblastLekara KreirajEntitet(string[] podaciEntiteta)
        {
            string id = podaciEntiteta[0];
            string nazivOblasti = podaciEntiteta[1];
            return new OblastLekara(id, nazivOblasti);
        }

        public override string KreirajTextZaUpis(OblastLekara entitet)
        {
            throw new NotImplementedException();
        }
    }
}
