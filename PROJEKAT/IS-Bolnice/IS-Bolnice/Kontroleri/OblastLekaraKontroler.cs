using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class OblastLekaraKontroler
    {
        private OblastLekaraServis oblastLekaraServis = new OblastLekaraServis();

        public List<OblastLekara> GetSveOblastiLekara()
        {
            return oblastLekaraServis.GetSveOblastiLekara();
        }
    }
}
