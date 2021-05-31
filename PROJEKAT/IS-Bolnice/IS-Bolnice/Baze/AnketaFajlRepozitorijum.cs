using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

namespace IS_Bolnice.Baze
{
    class AnketaFajlRepozitorijum : GenerickiFajlRepozitorijum<Anketa>, IAnketaRepozitorijum
    {
        private IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();

        private static string timeFormatForWriting = "M/d/yyyy h:mm:ss tt";
        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public AnketaFajlRepozitorijum() : base(@"..\..\Datoteke\Ocene.txt")
        {
        }

        public List<Anketa> GetSveAnketeBolnice()
        {
            List<Anketa> anketeBolnice = new List<Anketa>();
            foreach (Anketa anketa in DobaviSve())
            {
                if (anketa.Bolnica != null)
                {
                    anketeBolnice.Add(anketa);
                }
            }
            return anketeBolnice;
        }

        public List<Anketa> DobaviSveAnketeLekara()
        {
            List<Anketa> anketeBolnice = new List<Anketa>();
            foreach (Anketa anketa in DobaviSve())
            {
                if (anketa.Lekar != null)
                {
                    anketeBolnice.Add(anketa);
                }
            }
            return anketeBolnice;
        }

        public override Anketa KreirajEntitet(string[] podaciEntiteta)
        {
            string[] items = podaciEntiteta;

            if (podaciEntiteta[0] == "0")
            {
                return NapraviAnketuZaDoktora(items);
            }
            else
            {
                return NapraviAnketuZaBolnicu(items);
            }
        }

        private Anketa NapraviAnketuZaDoktora(string[] items)
        {
            Anketa survery = new Anketa();
            Lekar doctor = new Lekar();
            survery.Trajanje = FormirajDatumZaCitanje(items[1]);
            survery.Ocena = Int32.Parse(items[2]);
            survery.Komentar = items[3];
            survery.Pacijent = pacijentRepo.DobaviPoJmbg(items[4]);
            survery.Lekar = doctor;
            survery.Lekar.Jmbg = items[5];
            return survery;
        }

        private Anketa NapraviAnketuZaBolnicu(string[] items)
        {
            Anketa survery = new Anketa();
            Bolnica hospital = new Bolnica();
            survery.Trajanje = FormirajDatumZaCitanje(items[1]);
            survery.Ocena = Int32.Parse(items[2]);
            survery.Komentar = items[3];
            survery.Pacijent = pacijentRepo.DobaviPoJmbg(items[4]);
            survery.Bolnica = hospital;
            survery.Bolnica.Ime = items[5];
            return survery;
        }

        private DateTime FormirajDatumZaCitanje(string date)
        {
            return DateTime.ParseExact(date, timeFormatForReading, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override string KreirajTextZaUpis(Anketa entitet)
        {
            Anketa anketa = entitet;

            if(entitet.KojaAnketa == 0)
            {
                return "0" + "#" + FormirajDatumZaPisanje(anketa.Trajanje) + "#" + anketa.Ocena.ToString() + "#" + anketa.Komentar
                    + "#" + anketa.Pacijent.Jmbg + "#" + anketa.Lekar.Jmbg;
            }
            else
            {
                return "1" + "#" + FormirajDatumZaPisanje(anketa.Trajanje) + "#" + anketa.Ocena + "#" + anketa.Komentar
                    + "#" + anketa.Pacijent.Jmbg + "#" + anketa.Bolnica.Ime;
            }
        }

        private String FormirajDatumZaPisanje(DateTime date)
        {
            return date.ToString(timeFormatForWriting);
        }

    }

}
