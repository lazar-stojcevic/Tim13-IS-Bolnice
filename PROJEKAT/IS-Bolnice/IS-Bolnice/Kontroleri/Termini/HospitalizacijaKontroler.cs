using System.Collections.Generic;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Kontroleri.Termini
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
