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
    class BazaIzmena
    {
        private static string fileLocation = @"..\..\Datoteke\izmene.txt";
        private static string timeFormatForWriting = "M/d/yyyy h:mm:ss tt";
        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };
        private static int MAX_CHANGES_IN_WEEK = 3;
        public List<IzmenaTermina> FindPatientChanges(Pacijent patient)
        {
            List<IzmenaTermina> changesOfPatient = new List<IzmenaTermina>();

            foreach (IzmenaTermina change in ReadAllChanges())
            {
                if (IsJmbgEquals(change, patient))
                {
                    changesOfPatient.Add(change);
                }
            }

            return changesOfPatient;
        }

        public bool IsJmbgEquals(IzmenaTermina izmenaTermina, Pacijent patient)
        {
            return izmenaTermina.JmbgPacijenta.Equals(patient.Jmbg);
        }

        public void SaveChange(IzmenaTermina izmenaTermina)
        {
            List<string> lines = new List<string>();

            string formatOfchangeForWriting = ChangeToString(izmenaTermina);

            lines.Add(formatOfchangeForWriting);

            File.AppendAllLines(fileLocation, lines);
        }

        private string ChangeToString(IzmenaTermina izmenaTermina)
        {
            return izmenaTermina.JmbgPacijenta + "#" + GetFormatedDateForWriting(izmenaTermina.DatumIzmene);
        }

        public List<IzmenaTermina> ReadAllChanges()
        {
            List<string> lines = ReadLinesOfFile();
            List<IzmenaTermina> changes = new List<IzmenaTermina>();

            foreach (string line in lines)
            {
                changes.Add(MakeChangeFromLine(line));
            }

            return changes;
        }

        public List<string> ReadLinesOfFile()
        {
            return File.ReadAllLines(fileLocation).ToList(); ;
        }

        public IzmenaTermina MakeChangeFromLine(string line)
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
                changesString.Add(ChangeToString(change));
            }
            File.WriteAllLines(fileLocation, changesString);
        }

        public void UnblockPatient(Pacijent patient)
        {
            List<IzmenaTermina> allChanges = ReadAllChanges();
            List<IzmenaTermina> filteredChanges = new List<IzmenaTermina>();

            foreach (IzmenaTermina change in allChanges)
            {
                if (!IsJmbgEquals(change, patient))
                {
                    filteredChanges.Add(change);
                }
            }

            WriteAllChangesInFile(filteredChanges);
        }

        public bool IsPatientMalicious(Pacijent patient)
        {
            if (CountOfPatientChanges(patient) > MAX_CHANGES_IN_WEEK)
            {
                return true;
            }
            return false;
        }

        public int CountOfPatientChanges(Pacijent patient)
        {
            int numberOfPatientChanges = 0;
            foreach (IzmenaTermina change in FindPatientChanges(patient))
            {
                if (HasChangeHappenedInLastWeek(change))
                {
                    numberOfPatientChanges++;
                }
            }
            return numberOfPatientChanges;
        }

        public bool HasChangeHappenedInLastWeek(IzmenaTermina izmenaTermina)
        {
            DateTime now = DateTime.Now;
            System.DateTime lastWeekConstraint = now.AddDays(-7);

            if (izmenaTermina.DatumIzmene > lastWeekConstraint && izmenaTermina.DatumIzmene < now)
            {
                return true;
            }
            return false;
        }
    }
}
