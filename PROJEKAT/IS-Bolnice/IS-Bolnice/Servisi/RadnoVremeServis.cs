using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class RadnoVremeServis
    {
        private BazaRadnogVremena bazaRadnogVremena = new BazaRadnogVremena();

        public void IzmeniRadnoVreme(RadnoVremeLekara radnoVreme)
        {
            bazaRadnogVremena.IzmeniRadnoVreme(radnoVreme);
        }
    }
}
