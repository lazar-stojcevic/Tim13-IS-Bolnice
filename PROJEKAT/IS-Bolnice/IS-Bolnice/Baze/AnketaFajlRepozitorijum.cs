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

        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public AnketaFajlRepozitorijum() : base(@"..\..\Datoteke\Ocene.txt") { }

        public List<Anketa> GetSveAnketeBolnice()
        {
            List<Anketa> anketeBolnice = new List<Anketa>();
            foreach (Anketa anketa in GetSve())
            {
                if (anketa.Bolnica != null)
                {
                    anketeBolnice.Add(anketa);
                }
            }
            return anketeBolnice;
        }

        public List<Anketa> GetSveAnketeLekara()
        {
            List<Anketa> anketeBolnice = new List<Anketa>();
            foreach (Anketa anketa in GetSve())
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
            Anketa anketa = new Anketa();
            Lekar lekar = new Lekar();
            anketa.Trajanje = FormirajDatumZaCitanje(items[1]);
            anketa.Ocena = Int32.Parse(items[2]);
            anketa.Opis = items[3];
            anketa.Pacijent = pacijentRepo.GetPoJmbg(items[4]);
            anketa.Lekar = lekar;
            anketa.Lekar.Jmbg = items[5];
            return anketa;
        }

        private Anketa NapraviAnketuZaBolnicu(string[] items)
        {
            Anketa anketa = new Anketa();
            Bolnica bolnica = new Bolnica();
            anketa.Trajanje = FormirajDatumZaCitanje(items[1]);
            anketa.Ocena = Int32.Parse(items[2]);
            anketa.Opis = items[3];
            anketa.Pacijent = pacijentRepo.GetPoJmbg(items[4]);
            anketa.Bolnica = bolnica;
            anketa.Bolnica.Ime = items[5];
            return anketa;
        }

        private DateTime FormirajDatumZaCitanje(string datum)
        {
            return DateTime.ParseExact(datum, timeFormatForReading, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override string KreirajTextZaUpis(Anketa entitet)
        {
            return entitet.ToString();
        }
    }

}
