using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class SastojakServis
    {
        private BazaSastojaka bazaSastojaka = new BazaSastojaka();

        public List<Sastojak> GetSviSastojci()
        {
            return bazaSastojaka.DobaviSve();
        }

        internal void KreirajNoviSastojak(Sastojak noviSastojak)
        {
            bazaSastojaka.Sacuvaj(noviSastojak);
        }
    }
}
