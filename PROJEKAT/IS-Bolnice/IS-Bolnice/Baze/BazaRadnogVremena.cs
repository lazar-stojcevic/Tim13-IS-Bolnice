using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using IS_Bolnice.Model;

namespace IS_Bolnice.Baze
{
    class BazaRadnogVremena : GenerickiFajlRepozitorijum<RadnoVremeLekara>, IRadnoVremeRepozitorijum
    {
        private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
        private static string[] vremenskiFormatiCitanje = new[]
        {
            "M/d/yyyy h:mm:ss tt",
            "M-d-yyyy h:mm:ss tt"
        };

        public BazaRadnogVremena() : base(@"..\..\Datoteke\radnaVremenaLekara.txt")
        {
        }

        public override RadnoVremeLekara KreirajEntitet(string[] podaciEntiteta)
        {
            RadnoVremeLekara radnoVremeLekara = new RadnoVremeLekara()
            {
                Id = podaciEntiteta[0],
                StandardnoRadnoVreme = NapraviVremenskiInterval(podaciEntiteta[1]),
                VanrednaRadnaVremena = NapraviVanrednaRadnaVremena(podaciEntiteta[2]),
                SlobodniDani = NapraviSlobodneDane(podaciEntiteta[3]),
                SlobodniDaniUNedelji = NapraviSlobodneDaneUNedelji(podaciEntiteta[4]),
                PreostaliSlobodniDaniUGodini = Int32.Parse(podaciEntiteta[5])
            };
            return radnoVremeLekara;
        }

        public override string KreirajTextZaUpis(RadnoVremeLekara radnoVreme)
        {
            string radnoVremePisanje = radnoVreme.Id + "#" + radnoVreme.StandardnoRadnoVreme.Pocetak.ToString(vremenskiFormatPisanje)
                                       + "!" + radnoVreme.StandardnoRadnoVreme.Kraj.ToString(vremenskiFormatPisanje) + "#";

            // upisivanje liste vanrednih radnih vremena
            foreach (VremenskiInterval vanrednoRadnoVreme in radnoVreme.VanrednaRadnaVremena)
            {
                radnoVremePisanje += vanrednoRadnoVreme.Pocetak.ToString(vremenskiFormatPisanje) + "!"
                    + vanrednoRadnoVreme.Kraj.ToString(vremenskiFormatPisanje) + ",";

            }
            radnoVremePisanje = radnoVremePisanje.TrimEnd(',');
            radnoVremePisanje += "#";
            // upisivanje liste slobodnih dana
            foreach (DateTime slobodanDan in radnoVreme.SlobodniDani)
            {
                radnoVremePisanje += slobodanDan.ToString(vremenskiFormatPisanje) + ",";
            }
            radnoVremePisanje = radnoVremePisanje.TrimEnd(',');
            radnoVremePisanje += "#";

            // upisivanje liste slobodnih dana u nedelji
            foreach (DayOfWeek slobodanDanUNedelji in radnoVreme.SlobodniDaniUNedelji)
            {
                radnoVremePisanje += slobodanDanUNedelji.ToString() + ",";
            }
            radnoVremePisanje = radnoVremePisanje.TrimEnd(',');
            radnoVremePisanje += "#";

            // upisivanje preostalih slobodnih dana u godini
            radnoVremePisanje += radnoVreme.PreostaliSlobodniDaniUGodini.ToString();

            return radnoVremePisanje;
        }

        private VremenskiInterval NapraviVremenskiInterval(string radnoVreme)
        {
            string[] delovi = radnoVreme.Split('!');
            DateTime pocetak = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            DateTime kraj = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            return new VremenskiInterval(pocetak, kraj);
        }

        private List<VremenskiInterval> NapraviVanrednaRadnaVremena(string vanrednaRadnaVremenaString)
        {
            if (vanrednaRadnaVremenaString.Equals(""))
            {
                return new List<VremenskiInterval>();
            }

            List<VremenskiInterval> vanrednaRadnaVremena = new List<VremenskiInterval>();

            string[] delovi = vanrednaRadnaVremenaString.Split(',');
            foreach (string vremenskiIntervalString in delovi)
            {
                vanrednaRadnaVremena.Add(NapraviVremenskiInterval(vremenskiIntervalString));
            }

            return vanrednaRadnaVremena;
        }

        private List<DateTime> NapraviSlobodneDane(string slobodniDaniString)
        {
            if (slobodniDaniString.Equals(""))
            {
                return new List<DateTime>();
            }

            List<DateTime> slobodniDani = new List<DateTime>();

            string[] delovi = slobodniDaniString.Split(',');
            foreach (string deo in delovi)
            {
                DateTime datum = DateTime.ParseExact(deo, vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                slobodniDani.Add(datum);
            }

            return slobodniDani;
        }

        private List<DayOfWeek> NapraviSlobodneDaneUNedelji(string slobodniDaniUNedeljiString)
        {
            List<DayOfWeek> slobodniDaniUNedelji = new List<DayOfWeek>();

            if (slobodniDaniUNedeljiString.Equals(""))
            {
                return slobodniDaniUNedelji;
            }

            string[] delovi = slobodniDaniUNedeljiString.Split(',');
            foreach (string deo in delovi)
            {
                try
                {
                    slobodniDaniUNedelji.Add((DayOfWeek)Enum.Parse(typeof(DayOfWeek), deo));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Došlo je do greške prilikom parsiranja enuma za dane u nedelji.");
                    throw;
                }
            }

            return slobodniDaniUNedelji;
        }

        public RadnoVremeLekara RadnoVremeOdredjenogLekara(string jmbg)
        {
            List<RadnoVremeLekara> svaRadnaVremena = DobaviSve();

            foreach (RadnoVremeLekara radnoVreme in svaRadnaVremena)
            {
                if (radnoVreme.PripadaLekaru(jmbg))
                {
                    return radnoVreme;
                }
            }

            return null;
        }

        /*

        public RadnoVremeLekara RadnoVremeOdredjenogLekara(string jmbg)
        {
            
        }

        public void NovoRadnoVreme(RadnoVremeLekara novoRadnoVreme)
        {
            // lista se koristi samo zato sto je to potrebno za metodu AppendAllLines
            List<string> radnoVreme = new List<string>();
            radnoVreme.Add(RadnoVremeToString(novoRadnoVreme));
            File.AppendAllLines(fileLocation, radnoVreme);
        }

        public void IzmeniRadnoVreme(RadnoVremeLekara izmenjenoRadnoVreme)
        {
            List<RadnoVremeLekara> svaRadnaVremena = RadnoVremeSvihLekara();
            List<string> svaRandaVremenaString = new List<string>();
            foreach (RadnoVremeLekara radnoVreme in svaRadnaVremena)
            {
                if (radnoVreme.Id.Equals(izmenjenoRadnoVreme.Id))
                {
                    svaRandaVremenaString.Add(RadnoVremeToString(izmenjenoRadnoVreme));
                }
                else
                {
                    svaRandaVremenaString.Add(RadnoVremeToString(radnoVreme));
                }
            }
            File.WriteAllLines(fileLocation, svaRandaVremenaString);
        }

        public void ObrisiRadnoVreme(RadnoVremeLekara radnoVremeZaBrisanje)
        {
            List<RadnoVremeLekara> svaRadnaVremena = RadnoVremeSvihLekara();
            List<string> svaRandaVremenaString = new List<string>();
            foreach (RadnoVremeLekara radnoVreme in svaRadnaVremena)
            {
                if (radnoVreme.Id.Equals(radnoVremeZaBrisanje.Id))
                {
                    // ne unosi u listu
                }
                else
                {
                    svaRandaVremenaString.Add(RadnoVremeToString(radnoVreme));
                }
            }
            File.WriteAllLines(fileLocation, svaRandaVremenaString);
        }
        */

    }
}
