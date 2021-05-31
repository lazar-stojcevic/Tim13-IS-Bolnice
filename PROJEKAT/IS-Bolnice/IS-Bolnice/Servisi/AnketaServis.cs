using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IS_Bolnice.Servisi
{
    class AnketaServis
    {
        private readonly IAnketaRepozitorijum anketaRepo = new AnketaFajlRepozitorijum();
        private PreglediFajlRepozitorijum preglediFajlRepozitorijum = new PreglediFajlRepozitorijum();

        public List<Pregled> GetSviPreglediZaAnketu(string patientJmbg)
        {
            List<Pregled> pregledi = new List<Pregled>();
            foreach (Pregled pregled in GetSveAnketeKojeJePacijentImao(patientJmbg))
            {
                if (DaLiJePregledValidanZaAnketu(pregled))
                {
                    pregledi.Add(pregled);
                }
            }
            return pregledi;
        }

        public bool DaLiJeVremeZaAnketuBolnice(string jmbgPacijenta)
        {
            List<Anketa> sveAnketePacijentaZaBlonicu = GetSveAnketeBolnicePacijenta(jmbgPacijenta);

            if (sveAnketePacijentaZaBlonicu.Count == 0)
            {
                return true;
            }

            Anketa lastSurvery = NadjiPoslednjuAnketuBolnicePacijenta(sveAnketePacijentaZaBlonicu);

            return !DaLiJePacijentDostavioAnketuBolnice(lastSurvery);
        }

        public void SacuvajAnketu(Anketa anketa)
        {
            anketaRepo.Sacuvaj(anketa);
        }

        private bool DaLiJePacijentDostavioAnketuBolnice(Anketa anketa)
        {
            DateTime nextLegalDate = anketa.Trajanje.AddDays(30);
            if (DateTime.Now < nextLegalDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Anketa> GetSveAnketeBolnicePacijenta(string patientJmbg)
        {
            List<Anketa> hospitalSurveryOfPatient = new List<Anketa>();

            foreach (Anketa anketa in anketaRepo.GetSveAnketeBolnice())
            {
                if (anketa.Pacijent.Jmbg == patientJmbg)
                {
                    hospitalSurveryOfPatient.Add(anketa);
                }
            }

            hospitalSurveryOfPatient.Sort((x, y) => x.Trajanje.CompareTo(y.Trajanje));

            return hospitalSurveryOfPatient;
        }

        private Anketa NadjiPoslednjuAnketuBolnicePacijenta(List<Anketa> ankete)
        {
            return ankete.ElementAt(ankete.Count - 1);
        }

        private bool DaLiJePregledValidanZaAnketu(Pregled pregled)
        {
            DateTime dozvoljenoTrajanjeAnkete = pregled.VremeKrajaPregleda.AddDays(5);

            if (DateTime.Now < dozvoljenoTrajanjeAnkete && !DaLiJePregledVecOcenjen(pregled))
            {
                return true;
            }

            return false;
        }

        private bool DaLiJePregledVecOcenjen(Pregled pregled)
        {
            foreach (Anketa anketa in anketaRepo.DobaviSve())
            {
                if (anketa.Lekar != null)
                {
                    if (anketa.Lekar.Jmbg == pregled.Lekar.Jmbg && anketa.Pacijent.Jmbg == pregled.Pacijent.Jmbg && anketa.Trajanje == pregled.VremePocetkaPregleda)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<Pregled> GetSveAnketeKojeJePacijentImao(string pacijentJmbg)
        {
            List<Pregled> pastReviews = new List<Pregled>();

            foreach (Pregled pregled in preglediFajlRepozitorijum.DobaviSve())
            {
                if (pregled.Pacijent.Jmbg == pacijentJmbg && pregled.VremeKrajaPregleda < DateTime.Now)
                {
                    pastReviews.Add(pregled);
                }
            }
            return pastReviews;
        }
    }
}
