using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class OblastLekaraServis
    {
        private BazaOblastiLekara bazaOblastiLekara = new BazaOblastiLekara();

        public List<OblastLekara> GetSveOblastiLekara()
        {
            return bazaOblastiLekara.SveOblasti();
        }
    }
}
