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
    class IzmenaTerminaFajlRepozitorijum
    {
        private static string fileLocation = @"..\..\Datoteke\izmene.txt";
        private static string timeFormatForWriting = "M/d/yyyy h:mm:ss tt";
        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public List<IzmenaTermina> GetIzmenePacijenta(Pacijent patient)
        {
            List<IzmenaTermina> changesOfPatient = new List<IzmenaTermina>();

            foreach (IzmenaTermina change in ReadAllChanges())
            {
                if (DaLiJeJmbgJednak(change, patient))
                {
                    changesOfPatient.Add(change);
                }
            }

            return changesOfPatient;
        }

        private bool DaLiJeJmbgJednak(IzmenaTermina izmenaTermina, Pacijent pacijent)
        {
            return izmenaTermina.JmbgPacijenta.Equals(pacijent.Jmbg);
        }

        public void SaveChange(IzmenaTermina izmenaTermina)
        {
            List<string> lines = new List<string>();

            string formatOfchangeForWriting = FormatPisanjaIzmene(izmenaTermina);

            lines.Add(formatOfchangeForWriting);

            File.AppendAllLines(fileLocation, lines);
        }

        private string FormatPisanjaIzmene(IzmenaTermina izmenaTermina)
        {
            return izmenaTermina.JmbgPacijenta + "#" + GetFormatedDateForWriting(izmenaTermina.DatumIzmene);
        }

        public List<IzmenaTermina> ReadAllChanges()
        {
            List<string> lines = ReadLinesOfFile();
            List<IzmenaTermina> changes = new List<IzmenaTermina>();

            foreach (string line in lines)
            {
                changes.Add(NparaviIzmenuOdLinije(line));
            }

            return changes;
        }

        public List<string> ReadLinesOfFile()
        {
            return File.ReadAllLines(fileLocation).ToList(); ;
        }

        public IzmenaTermina NparaviIzmenuOdLinije(string line)
        {
            string[] items = line.Split('#');

            IzmenaTermina izmenaTermina = new IzmenaTermina();

            izmenaTermina.JmbgPacijenta = items[0];
            izmenaTermina.DatumIzmene = GetFormatedDateForReading(items[1]);

            return izmenaTermina;
        }

        public DateTime GetFormatedDateForReading(string date)
        {
            return DateTime.ParseExact(date, timeFormatForReading, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public String GetFormatedDateForWriting(DateTime date)
        {
            return date.ToString(timeFormatForWriting);
        }

        private void WriteAllChangesInFile(List<IzmenaTermina> changes)
        {
            List<String> changesString = new List<string>();
            foreach (IzmenaTermina change in changes)
            {
                changesString.Add(FormatPisanjaIzmene(change));
            }
            File.WriteAllLines(fileLocation, changesString);
        }

        public void OdblokirajPacijenta(Pacijent pacijent)
        {
            List<IzmenaTermina> sveIzmene = ReadAllChanges();
            List<IzmenaTermina> filtriraneIzmene = new List<IzmenaTermina>();

            foreach (IzmenaTermina izmena in sveIzmene)
            {
                if (!DaLiJeJmbgJednak(izmena, pacijent))
                {
                    filtriraneIzmene.Add(izmena);
                }
            }

            WriteAllChangesInFile(filtriraneIzmene);
        }
    }
}
