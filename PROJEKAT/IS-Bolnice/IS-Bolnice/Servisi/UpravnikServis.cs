using IS_Bolnice.Baze.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class UpravnikServis
    {
        IUpravnikRepozitorijum upravnikRepo = new UpravnikFajlRepozitorijum();

        public void Izmeni(Upravnik upravnik) {

            upravnikRepo.Izmeni(upravnik);
        }
    }
}
