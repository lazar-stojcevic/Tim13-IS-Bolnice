using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class SekretarServis
    {
        private ISekretarRepozitorijum repo = new Injector().GetSekretarRepozitorijum();
        
        public Sekretar GetByJmbg(string jmbg)
        {
            return repo.GetPoId(jmbg);
        }
    }
}
