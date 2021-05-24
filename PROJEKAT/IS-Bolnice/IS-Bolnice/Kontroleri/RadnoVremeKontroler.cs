using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class RadnoVremeKontroler
    {
        private RadnoVremeServis radnoVremeServis = new RadnoVremeServis();

        public void IzmeniRadnoVreme(RadnoVremeLekara radnoVreme)
        {
            radnoVremeServis.IzmeniRadnoVreme(radnoVreme);
        }
    }
}
