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
            return izvestajRepo.SviIzvestajiPacijenta(jmbgPacijenta);
        }

        public List<Izvestaj> DobaviSveIzvestaje()
        {
            return izvestajRepo.DobaviSve();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihNedeljuDana()
        {
           return izvestajRepo.DobaviSveIzvestajeizPoslednjihNedeljuDana();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihMesecDana()
        {
            return izvestajRepo.DobaviSveIzvestajeizPoslednjihMesecDana();
        }

        public List<Izvestaj> DobaviSveIzvestajeizPoslednjihGodinuDana()
        {
            return izvestajRepo.DobaviSveIzvestajeizPoslednjihGodinuDana();
        }
    }
}
