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

        public List<Beleska> SveBelezke()
        {
            List<Beleska> belezke = new List<Beleska>();

            foreach (var linija in ProcitajDatoteku())
            {
                string[] deloviLinije = linija.Split('#');
                belezke.Add(FormatCitanjaBelezke(deloviLinije));
            }

            return belezke;
        }

        public void SacuvajBelezku(Beleska beleska)
        {
            List<string> linije = new List<string>();

            linije.Add(FormatPisanjaBelezke(beleska));

            File.AppendAllLines(filePath, linije);
        }

        public void ObrisiBelezku(Beleska beleskaZaBrisanje)
        {
            List<string> belezkeKojeSeNeBrisu = new List<string>();

            foreach (Beleska belezka in SveBelezke())
            {
                if (!belezka.Iste(beleskaZaBrisanje))
                {
                    belezkeKojeSeNeBrisu.Add(FormatPisanjaBelezke(belezka));
                }
            }

            File.WriteAllLines(filePath, belezkeKojeSeNeBrisu);
        }

        public void IzmeniBelezku(Beleska staraBeleska, Beleska novaBeleska)
        {
            List<string> regularneBelezke = new List<string>();

            foreach (Beleska belezka in SveBelezke())
            {
                if (!belezka.Iste(staraBeleska))
                {
                    regularneBelezke.Add(FormatPisanjaBelezke(belezka));
                }
                else
                {
                    regularneBelezke.Add(FormatPisanjaBelezke(novaBeleska));
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

        private string FormatPisanjaBelezke(Beleska beleska)
        {
            return beleska.Pacijent.Jmbg + "#" + beleska.Komentar + "#" + beleska.PeriodVazenja.ToString() + "#" + FormirajDatumZaPisanje(beleska.VremePocetkaVazenja) + "#" + beleska.Naziv;
        }

        private Beleska FormatCitanjaBelezke(string[] linija)
        {
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = linija[0];

            return new Beleska(pacijent, linija[1], FormirajDatumZaCitanje(linija[3]), Int32.Parse(linija[2]), linija[4]);
        }

        private List<string> ProcitajDatoteku()
        {
            return File.ReadAllLines(filePath).ToList();
        }

    }
}
