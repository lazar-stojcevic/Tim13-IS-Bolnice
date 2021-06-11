using IS_Bolnice.Baze.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class UpravnikServis
    {
        IUpravnikRepozitorijum upravnikRepo = new Injector().GetUpravnikRepozitorijum();

        public void Izmeni(Upravnik upravnik) {

            upravnikRepo.Izmeni(upravnik);
        }


        public Upravnik GetByJmbg(string jmbg)
        {
            return upravnikRepo.GetPoId(jmbg);
        }
    }
}
