using System;
using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Termini
{
    class HospitalizacijaServis
    {
        private IHospitalizacijaRepozitorijum hospitalizacijaRepo = new Injector().GetHospitalizacijaRepozitorijum();

        public List<Hospitalizacija> GetSveHospitalizacije()
        {
            return hospitalizacijaRepo.GetSve();
        }

        public int GetBrojTrenutnoHospitalizovanjihUSobi(string idSobe)
        {
            int brojTrenutnoHospitalizovanih = 0;
            foreach (Hospitalizacija hospitalizacija in GetSveHospitalizacije()) 
            {
                if (DateTime.Now > hospitalizacija.PocetakHospitalizacije && DateTime.Now < hospitalizacija.KrajHospitalizacije) {
                    brojTrenutnoHospitalizovanih++;
                }
            }
            return brojTrenutnoHospitalizovanih;
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
