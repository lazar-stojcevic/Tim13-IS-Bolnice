using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;

namespace IS_Bolnice.Repozitorijumi
{
    class IzmenaTerminaFajlRepozitorijum : GenerickiFajlRepozitorijum<IzmenaTermina>, IIzmenaTerminaRepozitorijum
    {
        private static string fileLocation = @"..\..\Datoteke\izmene.txt";
        private static string timeFormatForWriting = "M/d/yyyy h:mm:ss tt";
        private static string[] timeFormatForReading = new[]
        {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
        };

        public IzmenaTerminaFajlRepozitorijum() : base(fileLocation)
        {
        }

        public bool DaLiJeJmbgJednak(IzmenaTermina izmenaTermina, Pacijent pacijent)
        {
            return izmenaTermina.JmbgPacijenta.Equals(pacijent.Jmbg);
        }

        public List<IzmenaTermina> GetIzmenePacijenta(Pacijent pacijent)
        {
            List<IzmenaTermina> changesOfPatient = new List<IzmenaTermina>();

            foreach (IzmenaTermina change in GetSve())
            {
                if (DaLiJeJmbgJednak(change, pacijent))
                {
                    changesOfPatient.Add(change);
                }
            }

            return changesOfPatient;
        }

        public override IzmenaTermina KreirajEntitet(string[] podaciEntiteta)
        {
            IzmenaTermina izmenaTermina = new IzmenaTermina();

            izmenaTermina.JmbgPacijenta = podaciEntiteta[0];
            izmenaTermina.DatumIzmene = FormatCitanjaDatuma(podaciEntiteta[1]);

            return izmenaTermina;
        }

        public override string KreirajTextZaUpis(IzmenaTermina entitet)
        {
            IzmenaTermina izmenaTermina = entitet;
            return izmenaTermina.JmbgPacijenta + "#" + FormatPisanjaDatuma(izmenaTermina.DatumIzmene);
        }

        public void OdblokirajPacijenta(Pacijent pacijent)
        {
            List<IzmenaTermina> sveIzmene = GetSve();
            List<IzmenaTermina> filtriraneIzmene = new List<IzmenaTermina>();

            foreach (IzmenaTermina izmena in sveIzmene)
            {
                if (!DaLiJeJmbgJednak(izmena, pacijent))
                {
                    filtriraneIzmene.Add(izmena);
                }
            }

            UpisiSveIzmene(filtriraneIzmene);
        }

        private void UpisiSveIzmene(List<IzmenaTermina> izmene)
        {
            List<string> izmeneString = new List<string>();
            foreach (IzmenaTermina izmena in izmene)
            {
                izmeneString.Add(KreirajTextZaUpis(izmena));
            }
            File.WriteAllLines(fileLocation, izmeneString);
        }

        private DateTime FormatCitanjaDatuma(string datum)
        {
            return DateTime.ParseExact(datum, timeFormatForReading, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        private string FormatPisanjaDatuma(DateTime datum)
        {
            return datum.ToString(timeFormatForWriting);
        }
    }
}
