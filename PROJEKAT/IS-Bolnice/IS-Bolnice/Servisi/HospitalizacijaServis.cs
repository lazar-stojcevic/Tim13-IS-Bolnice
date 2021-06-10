using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class HospitalizacijaServis
    {
        private IHospitalizacijaRepozitorijum hospitalizacijaRepo = new Injector().GetHospitalizacijaRepozitorijum();

        public List<Hospitalizacija> GetSveHospitalizacije()
        {
            return hospitalizacijaRepo.GetSve();
        }

        public bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            return hospitalizacijaRepo.KreirajHospitalizaciju(hospitalizacija);
        }

        public void ObrisiHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            hospitalizacijaRepo.Obrisi(hospitalizacija.Id);
        }
    }
}
