using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class HospitalizacijaKontroler
    {
        private HospitalizacijaServis hospitalizacijaServis = new HospitalizacijaServis();

        public List<Hospitalizacija> GetSveHospitalizacije()
        {
            return hospitalizacijaServis.GetSveHospitalizacije();
        }

        public bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            return hospitalizacijaServis.KreirajHospitalizaciju(hospitalizacija);
        }

        public void ObrisiHospitalizaciju(Hospitalizacija hospitalizacija)
        {
            hospitalizacijaServis.ObrisiHospitalizaciju(hospitalizacija);
        }
    }
}
