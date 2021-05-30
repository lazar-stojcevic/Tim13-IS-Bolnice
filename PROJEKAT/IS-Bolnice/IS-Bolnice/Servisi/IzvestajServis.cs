using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class IzvestajServis
    {
        private IIzvestajRepozitorijum izvestajRepo = new IzvestajFajlRepozitorijum();

        public void KreirajIzvestaj(Izvestaj izvestaj)
        {
            izvestajRepo.Sacuvaj(izvestaj);
        }

        public List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta)
        {
            List<Izvestaj> izvestajiPacijenta = new List<Izvestaj>();

            foreach (Izvestaj izvestaj in izvestajRepo.DobaviSve())
            {
                if (izvestaj.Pacijent.Jmbg == jmbgPacijenta)
                {
                    izvestajiPacijenta.Add(izvestaj);
                }
            }

            return izvestajiPacijenta;
        }
    }
}
