using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Baze
{
    //JEDNOM U 30 DANA JE MOGUCE OCENITI BOLNICU
    class BazaOcena
    {
        private static string fileLocation = @"..\..\Datoteke\Ocene.txt";
        private BazaPregleda bazaPregleda = new BazaPregleda();
        private LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();
        private IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();

        private static string timeFormatForWriting = "M/d/yyyy h:mm:ss tt";
        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public List<Survery> FindAllHospitalSurvery()
        {
            List<Survery> hospitalSurveries = new List<Survery>();
            foreach (Survery survery in ReadAllSurvery())
            {
                if (survery.Hospital != null)
                {
                    hospitalSurveries.Add(survery);
                }
            }
            return hospitalSurveries;
        }

        public bool IsTimeForHospitalSurvery(string jmbgPacijenta)
        {
            List<Survery> allHospitalSurveryesOfPatient = FindAllHospitalSurveryOfPatient(jmbgPacijenta);

            if (allHospitalSurveryesOfPatient.Count == 0)
            {
                return true;
            }

            Survery lastSurvery = FindLastHospitalSurveryOfPatient(allHospitalSurveryesOfPatient);

            return !IsHospitalSurveryOfPatientDelivered(lastSurvery);
        }

        public bool IsHospitalSurveryOfPatientDelivered(Survery survery)
        {
            DateTime nextLegalDate = survery.TimeLimit.AddDays(30);
            if (DateTime.Now < nextLegalDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Survery> FindAllHospitalSurveryOfPatient(string patientJmbg)
        {
            List<Survery> hospitalSurveryOfPatient = new List<Survery>();

            foreach (Survery survery in FindAllHospitalSurvery())
            {
                if (survery.Patient.Jmbg == patientJmbg)
                {
                    hospitalSurveryOfPatient.Add(survery);
                }
            }

            hospitalSurveryOfPatient.Sort((x, y) => x.TimeLimit.CompareTo(y.TimeLimit));

            return hospitalSurveryOfPatient;
        }

        public Survery FindLastHospitalSurveryOfPatient(List<Survery> surveries)
        {
            return surveries.ElementAt(surveries.Count - 1);
        }

        public List<Survery> ReadAllSurvery()
        {
            List<Survery> grades = new List<Survery>();

            foreach (string line in ReadLinesOfFile())
            {
                grades.Add(MakeSurveryFromLine(line));
            }

            return grades;
        }

        public List<string> ReadLinesOfFile()
        {
            return File.ReadAllLines(fileLocation).ToList(); ;
        }

        public Survery MakeSurveryFromLine(string line)
        {
            string[] items = line.Split('#');
            if (items[0] == "0")
            {
                return MakeSurveryForDoctor(items);
            }
            else
            {
                return MakeSurveryForHospital(items);
            }
        }

        public Survery MakeSurveryForDoctor(string[] items)
        {
            Survery survery = new Survery();
            Lekar doctor = new Lekar();
            survery.TimeLimit = GetFormatedDateForReading(items[1]);
            survery.Rating = Int32.Parse(items[2]);
            survery.Comment = items[3];
            survery.Patient = pacijentRepo.DobaviPoJmbg(items[4]);
            survery.Doctor = doctor;
            survery.Doctor.Jmbg = items[5];
            return survery;
        }

        public Survery MakeSurveryForHospital(string[] items)
        {
            Survery survery = new Survery();
            Bolnica hospital = new Bolnica();
            survery.TimeLimit = GetFormatedDateForReading(items[1]);
            survery.Rating = Int32.Parse(items[2]);
            survery.Comment = items[3];
            survery.Patient = pacijentRepo.DobaviPoJmbg(items[4]);
            survery.Hospital = hospital;
            survery.Hospital.Ime = items[5];
            survery.Doctor = new Lekar();
            survery.Doctor.Jmbg = "aaa"; //OVO SAM SREDIO OVAKO, TI POSLE POPRAVI
            return survery;
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

        public void SaveSurvery(Survery survery, int whatSurvery)
        {

            if (whatSurvery == 0)
            {
                SavingSurveryForDoctor(survery);
            }
            else
            {
                SavingSurveryForHospital(survery);
            }
        }

        public void SavingSurveryForDoctor(Survery survery)
        {
            List<string> lines = new List<string>();

            string formatForSavingSurvery = GetFormatForSavingSurveryForDoctror(survery);

            lines.Add(formatForSavingSurvery);

            File.AppendAllLines(fileLocation, lines);
        }

        public string GetFormatForSavingSurveryForDoctror(Survery survery)
        {
            return "0" + "#" + GetFormatedDateForWriting(survery.TimeLimit) + "#" + survery.Rating.ToString() + "#" + survery.Comment
                + "#" + survery.Patient.Jmbg + "#" + survery.Doctor.Jmbg;
        }

        public void SavingSurveryForHospital(Survery survery)
        {
            List<string> lines = new List<string>();

            string formatForSavingSurvery = GetFormatForSavingSurveryForHospital(survery);

            lines.Add(formatForSavingSurvery);

            File.AppendAllLines(fileLocation, lines);
        }

        public string GetFormatForSavingSurveryForHospital(Survery survery)
        {
            return "1" + "#" + GetFormatedDateForWriting(survery.TimeLimit) + "#" + survery.Rating + "#" + survery.Comment
                + "#" + survery.Patient.Jmbg + "#" + survery.Hospital.Ime;
        }

        public List<Pregled> AllReviewForSurvery(string patientJmbg)
        {
            List<Pregled> reviews = new List<Pregled>();
            Console.WriteLine("AllReview");
            foreach (Pregled review in AllPastReviewOfPatient(patientJmbg))
            {
                if (IsReviewValidForSurvery(review))
                {
                    reviews.Add(review);
                }
            }
            return reviews;
        }

        public bool IsReviewValidForSurvery(Pregled review)
        {
            DateTime legalTimeForSurvery = review.VremeKrajaPregleda.AddDays(5);

            if (DateTime.Now < legalTimeForSurvery && !IsSurveryForReviewDelivered(review))
            {
                return true;
            }

            return false;
        }

        public bool IsSurveryForReviewDelivered(Pregled review)
        {
            foreach (Survery survery in ReadAllSurvery())
            {
                if (survery.Doctor != null)
                {
                    if (survery.Doctor.Jmbg == review.Lekar.Jmbg && survery.Patient.Jmbg == review.Pacijent.Jmbg && survery.TimeLimit == review.VremePocetkaPregleda)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Lekar FindDoctorFromJmbg(string doctorJmbg)
        {
            foreach (Lekar doctor in lekarFajlRepozitorijum.DobaviSve())
            {
                if (doctor.Jmbg == doctorJmbg)
                {
                    return doctor;
                }
            }
            return null;
        }

        public List<Pregled> AllPastReviewOfPatient(string patientJmbg)
        {
            List<Pregled> pastReviews = new List<Pregled>();

            foreach (Pregled review in bazaPregleda.SviBuduciPregledi())
            {
                if (review.Pacijent.Jmbg == patientJmbg && review.VremeKrajaPregleda < DateTime.Now)
                {
                    pastReviews.Add(review);
                }
            }
            return pastReviews;
        }
    }
}
