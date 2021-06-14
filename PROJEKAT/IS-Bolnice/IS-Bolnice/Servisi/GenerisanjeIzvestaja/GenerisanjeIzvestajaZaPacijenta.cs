using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Servisi.GenerisanjeIzvestaja
{
    public abstract class GenerisanjeIzvestajaZaPacijenta
    {
        protected List<Operacija> operacijePacijenta;
        public bool GenerisanjeIzvestajaBuducihOperacijaPacijenta(string jmbgPacijenta)
        {
            OperacijaServis operacijaServis = new OperacijaServis();
            operacijePacijenta = operacijaServis.GetSveBuduceOperacijePacijenta(jmbgPacijenta);
            SortirajTermine();
            return GenerisiIzvestajBuducihOperacijaPacijenta();
        }

        protected abstract bool GenerisiIzvestajBuducihOperacijaPacijenta();

        protected void SortirajTermine()
        {

        }
    }
}
