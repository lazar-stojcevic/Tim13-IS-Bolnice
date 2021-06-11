using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class SekretarKontroler
    {
        private SekretarServis sekretarServis = new SekretarServis();

        public Sekretar GetByJmbg(string jmbg)
        {
            return sekretarServis.GetByJmbg(jmbg);
        }
    }
}
