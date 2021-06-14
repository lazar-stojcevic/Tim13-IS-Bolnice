using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Servisi.Informativni
{
    class IzvestajServis
    {
        private IIzvestajRepozitorijum izvestajRepo = new Injector().GetIzvestajRepozitorijum();

        public void KreirajIzvestaj(Izvestaj izvestaj)
        {
            izvestajRepo.Sacuvaj(izvestaj);
        }

        public List<Izvestaj> GetSviIzvestajiPacijenta(string jmbgPacijenta)
        {
            return izvestajRepo.SviIzvestajiPacijenta(jmbgPacijenta);
        }

        public List<Izvestaj> GetSviIzvestaji()
        {
            return izvestajRepo.GetSve();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihNedeljuDana()
        {
           return izvestajRepo.GetSviIzvestajiIzPoslednjihNedeljuDana();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihMesecDana()
        {
            return izvestajRepo.GetSviIzvestajiIzPoslednjihMesecDana();
        }

        public List<Izvestaj> GetSviIzvestajiIzPoslednjihGodinuDana()
        {
            return izvestajRepo.GetSviIzvestajiIzPoslednjihGodinuDana();
        }
    }
}
