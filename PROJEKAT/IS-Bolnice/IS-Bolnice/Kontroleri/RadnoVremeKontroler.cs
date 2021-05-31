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

        public int PreracunajBrojIskoriscenihSlobodnihDanaLekara(string jmbgLekara, List<DateTime> noviSlobodniDani)
        {
            return radnoVremeServis.PreracunajBrojIskoriscenihSlobodnihDanaLekara(jmbgLekara, noviSlobodniDani);
        }

        public bool PreklapanjeIntervalaGodisnjegOdmoraLekara(List<DateTime> potencijalniSlobodniDani, string jmbgLekara)
        {
            return radnoVremeServis.PreklapanjeIntervalaGodisnjegOdmoraLekara(potencijalniSlobodniDani, jmbgLekara);
        }

        public bool PreklapanjeIntervalaGodisnjegOdmoraSaObavezamaLekara(List<DateTime> daniGodisnjegOdmora,
            string jmbgLekara)
        {
            int poslednji = daniGodisnjegOdmora.Count - 1;
            VremenskiInterval intervalGodisnjegOdmora =
                new VremenskiInterval(daniGodisnjegOdmora[0], daniGodisnjegOdmora[poslednji]);
            return radnoVremeServis.PreklapanjeIntervalaGodisnjegOdmoraSaObavezamaLekara(intervalGodisnjegOdmora,
                jmbgLekara);
        }

        public RadnoVremeLekara DobaviRadnoVremeLekara(string idLekara)
        {
            return radnoVremeServis.DobaRadnoVremeLekara(idLekara);
        }
    }
}
