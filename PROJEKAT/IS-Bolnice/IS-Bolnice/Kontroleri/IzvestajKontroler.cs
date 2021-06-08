using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class IzvestajKontroler
    {
        private IzvestajServis izvestajServis = new IzvestajServis();

        public void KreirajIzvestaj(Izvestaj izvestaj)
        {
            izvestajServis.KreirajIzvestaj(izvestaj);
        }

        public List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta)
        {
            return izvestajServis.SviIzvestajiPacijenta(jmbgPacijenta);
        }

        public List<Izvestaj> DobaviSveIzvestaje()
        {
            return izvestajServis.DobaviSveIzvestaje();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihNedeljuDana()
        {
            return izvestajServis.DobaviSveIzvestajeizPoslednjihNedeljuDana();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihMesecDana()
        {
            return izvestajServis.DobaviSveIzvestajeizPoslednjihMesecDana();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihGodinuDana()
        {
            return izvestajServis.DobaviSveIzvestajeizPoslednjihGodinuDana();
        }
    }
}
