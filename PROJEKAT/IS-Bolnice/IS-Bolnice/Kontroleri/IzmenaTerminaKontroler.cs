using IS_Bolnice.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Kontroleri
{
    class IzmenaTerminaKontroler
    {
        private IzmenaTerminaServis izmenaTerminaServis = new IzmenaTerminaServis();

        public bool DaLiJePacijentMaliciozan(Pacijent pacijent) {
            return izmenaTerminaServis.DaLiJePacijentMaliciozan(pacijent);
        }

        public void OdblokirajPacijenta(Pacijent pacijent)
        {
            izmenaTerminaServis.OdblokirajPacijenta(pacijent);
        }
    }
}
