using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze
{
    class BazaBelezaka
    {
        private static string filePath = @"..\..\Datoteke\belezke.txt";
        private static string formatPisanjaDatuma = "M/d/yyyy h:mm:ss tt";
        private static string[] formatCitanjaDatuma = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public List<Belezka> SveBelezke()
        {
            List<Belezka> belezke = new List<Belezka>();

            foreach (var linija in ProcitajDatoteku())
            {
                string[] deloviLinije = linija.Split('#');
                belezke.Add(FormatCitanjaBelezke(deloviLinije));
            }

            return belezke;
        }

        public void SacuvajBelezku(Belezka belezka)
        {
            List<string> linije = new List<string>();

            linije.Add(FormatPisanjaBelezke(belezka));

            File.AppendAllLines(filePath, linije);
        }

        public void ObrisiBelezku(Belezka belezkaZaBrisanje)
        {
            List<string> belezkeKojeSeNeBrisu = new List<string>();

            foreach (Belezka belezka in SveBelezke())
            {
                if (!belezka.Iste(belezkaZaBrisanje))
                {
                    belezkeKojeSeNeBrisu.Add(FormatPisanjaBelezke(belezka));
                }
            }

            File.WriteAllLines(filePath, belezkeKojeSeNeBrisu);
        }

        public void IzmeniBelezku(Belezka staraBelezka, Belezka novaBelezka)
        {
            List<string> regularneBelezke = new List<string>();

            foreach (Belezka belezka in SveBelezke())
            {
                if (!belezka.Iste(staraBelezka))
                {
                    regularneBelezke.Add(FormatPisanjaBelezke(belezka));
                }
                else
                {
                    regularneBelezke.Add(FormatPisanjaBelezke(novaBelezka));
                }
            }

            File.WriteAllLines(filePath, regularneBelezke);
        }

        private DateTime FormirajDatumZaCitanje(string datum)
        {
            return DateTime.ParseExact(datum, formatCitanjaDatuma, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        private string FormirajDatumZaPisanje(DateTime date)
        {
            return date.ToString(formatPisanjaDatuma);
        }

        private string FormatPisanjaBelezke(Belezka belezka)
        {
            return belezka.Pacijent.Jmbg + "#" + belezka.Komentar + "#" + belezka.PeriodVazenja.ToString() + "#" + FormirajDatumZaPisanje(belezka.VremePocetkaVazenja) + "#" + belezka.Naziv;
        }

        private Belezka FormatCitanjaBelezke(string[] linija)
        {
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = linija[0];

            return new Belezka(pacijent, linija[1], FormirajDatumZaCitanje(linija[3]), Int32.Parse(linija[2]), linija[4]);
        }

        private List<string> ProcitajDatoteku()
        {
            return File.ReadAllLines(filePath).ToList();
        }

    }
}
