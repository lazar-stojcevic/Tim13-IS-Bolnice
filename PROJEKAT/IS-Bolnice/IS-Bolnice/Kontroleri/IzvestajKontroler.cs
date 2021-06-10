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

        public List<Izvestaj> GetSviIzvestajiPacijenta(string jmbgPacijenta)
        {
            return izvestajServis.GetSviIzvestajiPacijenta(jmbgPacijenta);
        }

        public List<Izvestaj> GetSviIzvestaji()
        {
            return izvestajServis.GetSviIzvestaji();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihNedeljuDana()
        {
            return izvestajServis.GetSviIzvestajiIzPoslednjihNedeljuDana();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihMesecDana()
        {
            return izvestajServis.GetSviIzvestajiIzPoslednjihMesecDana();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihGodinuDana()
        {
            return izvestajServis.GetSviIzvestajiIzPoslednjihGodinuDana();
        }
    }
}
