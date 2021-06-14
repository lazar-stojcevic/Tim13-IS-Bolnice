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
    class BeleskaFajlRepozitorijum : GenerickiFajlRepozitorijum<Beleska>, IBeleskaRepozitorijum
    {
        private static string formatPisanjaDatuma = "M/d/yyyy h:mm:ss tt";
        private static string[] formatCitanjaDatuma = new[]
        {
            "M/d/yyyy h:mm:ss tt",
            "M-d-yyyy h:mm:ss tt"
        };

        public BeleskaFajlRepozitorijum() : base(@"..\..\Datoteke\belezke.txt")
        {
        }

        public override Beleska KreirajEntitet(string[] podaciEntiteta)
        {
            PacijentFajlRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();
            Pacijent pacijent = pacijentRepo.GetPoJmbg(podaciEntiteta[1]);

            return new Beleska(podaciEntiteta[0], pacijent, podaciEntiteta[2], FormirajDatumZaCitanje(podaciEntiteta[4]), Int32.Parse(podaciEntiteta[3]),
                podaciEntiteta[5]);
        }

        public override string KreirajTextZaUpis(Beleska entitet)
        {
            return entitet.Id + "#" + entitet.Pacijent.Jmbg + "#" + entitet.Komentar + "#" + entitet.PeriodVazenja.ToString() + "#" 
                   + FormirajDatumZaPisanje(entitet.VremePocetkaVazenja) + "#" + entitet.Naziv;
        }

        private string FormirajDatumZaPisanje(DateTime date)
        {
            return date.ToString(formatPisanjaDatuma);
        }

        private DateTime FormirajDatumZaCitanje(string datum)
        {
            return DateTime.ParseExact(datum, formatCitanjaDatuma, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
    }
}
