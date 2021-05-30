using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;

namespace IS_Bolnice.Servisi
{
    class HospitalizacijaServis
    {
        private BazaHospitalizacija bazaHospitalizacija = new BazaHospitalizacija();

        public List<Hospitalizacija> GetSveHospitalizacije()
        {
            return bazaHospitalizacija.DobaviSve();
        }

        public bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            return bazaHospitalizacija.KreirajHospitalizaciju(hospitalizacija);
        }

        public void ObrisiHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            bazaHospitalizacija.Obrisi(hospitalizacija.Id);
        }
    }
}
