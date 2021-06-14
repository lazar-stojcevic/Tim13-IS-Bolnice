using System;
using System.Collections.Generic;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Korisnicki;

namespace IS_Bolnice.Kontroleri.Korisnicki
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

        public RadnoVremeLekara GetRadnoVremeLekara(string idLekara)
        {
            return radnoVremeServis.GetRadnoVremeLekara(idLekara);
        }

        public void OdloziSaObavestenjemTermineLekaraKojiSeNeUklapaju(VremenskiInterval vremenskiInterval, String jmbgLekara)
        {
            radnoVremeServis.OdloziSaObavestenjemTermineLekaraKojiSeNeUklapaju(vremenskiInterval, jmbgLekara);
        }

        public void OdloziSaObavestenjemBuduceTermineLekaraZbogPromene(Lekar lekar)
        {
            radnoVremeServis.OdloziSaObavestenjemBuduceTermineLekaraZbogPromene(lekar);
        }

        public bool MozeLiSeDodelitiSlobodanDanUNedeljiLekaru(Lekar lekar, DayOfWeek dan)
        {
            return radnoVremeServis.MozeLiSeDodelitiSlobodanDanUNedeljiLekaru(lekar, dan);
        }
    }
}
