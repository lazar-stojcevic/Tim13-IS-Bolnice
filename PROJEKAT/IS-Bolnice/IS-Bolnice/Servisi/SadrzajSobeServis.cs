using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class SadrzajSobeServis
    {
        private BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();

        public List<SadrzajSobe> GetSadrzajSobe(string idSobe)
        {
            return bazaSadrzaja.GetSadrzajSobe(idSobe);
        }
    }
}
