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
        public List<Change> FindPatientChanges(Pacijent patient)
        {
            List<Change> changesOfPatient = new List<Change>();

            foreach (Change change in ReadAllChanges())
            {
                if (IsJmbgEquals(change, patient))
                {
                    changesOfPatient.Add(change);
                }
            }

            return changesOfPatient;
        }

        public bool IsJmbgEquals(Change change, Pacijent patient)
        {
            return change.JmbgOfPatient.Equals(patient.Jmbg);
        }

        public void SaveChange(Change change)
        {
            List<string> lines = new List<string>();

            string formatOfchangeForWriting = ChangeToString(change);

            lines.Add(formatOfchangeForWriting);

            File.AppendAllLines(fileLocation, lines);
        }

        private string ChangeToString(Change change)
        {
            return change.JmbgOfPatient + "#" + GetFormatedDateForWriting(change.DateOfChange);
        }

        public List<Change> ReadAllChanges()
        {
            List<string> lines = ReadLinesOfFile();
            List<Change> changes = new List<Change>();

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

        public Change MakeChangeFromLine(string line)
        {
            string[] items = line.Split('#');

            Change change = new Change();

            change.JmbgOfPatient = items[0];
            change.DateOfChange = GetFormatedDateForReading(items[1]);

            return change;
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

        private void WriteAllChangesInFile(List<Change> changes)
        {
            List<String> changesString = new List<string>();
            foreach (Change change in changes)
            {
                changesString.Add(ChangeToString(change));
            }
            File.WriteAllLines(fileLocation, changesString);
        }

        public void UnblockPatient(Pacijent patient)
        {
            List<Change> allChanges = ReadAllChanges();
            List<Change> filteredChanges = new List<Change>();

            foreach (Change change in allChanges)
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
            foreach (Change change in FindPatientChanges(patient))
            {
                if (HasChangeHappenedInLastWeek(change))
                {
                    numberOfPatientChanges++;
                }
            }
            return numberOfPatientChanges;
        }

        public bool HasChangeHappenedInLastWeek(Change change)
        {
            DateTime now = DateTime.Now;
            System.DateTime lastWeekConstraint = now.AddDays(-7);

            if (change.DateOfChange > lastWeekConstraint && change.DateOfChange < now)
            {
                return true;
            }
            return false;
        }
    }
}
