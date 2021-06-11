using System;
using System.Collections.Generic;

namespace IS_Bolnice.Model
{
    public class RadnoVremeLekara : Entitet
    {
        public VremenskiInterval StandardnoRadnoVreme { get; set; }
        public List<VremenskiInterval> VanrednaRadnaVremena { get; set; }
        public List<DateTime> SlobodniDani { get; set; }
        public List<DayOfWeek> SlobodniDaniUNedelji { get; set; }

        public int PreostaliSlobodniDaniUGodini { get; set; }

        public RadnoVremeLekara() : base()
        {
            VanrednaRadnaVremena = new List<VremenskiInterval>();
            SlobodniDani = new List<DateTime>();
            SlobodniDaniUNedelji = new List<DayOfWeek>();
            PreostaliSlobodniDaniUGodini = 30;
        }

        public bool PripadaLekaru(string jmbg)
        {
            return this.Id.Equals(jmbg);
        }

        public bool VecDodeljenoVanrednoRadnoVremeLekaru(VremenskiInterval interval)
        {
            foreach (VremenskiInterval vi in VanrednaRadnaVremena)
            {
                if (vi.DaLiSePreklapaSa(interval))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TerminURadnomVremenuLekara(VremenskiInterval termin)
        {
            if (TerminUGodisnjemOdmoruLekara(termin))
            {
                return false;
            }

            if (TerminUVanrednomRadnomVremenuLekara(termin))
            {
                return true;
            }

            if (TerminUSlobodnomDanuNedeljeLekara(termin))
            {
                return false;
            }

            if (TerminUStandardnomRadnomVremenuLekara(termin) && !DefinisanoVanrednoRadnoVremeLekaraZa(termin))
            {
                return true;
            }

            return false;
        }

        private bool TerminUSlobodnomDanuNedeljeLekara(VremenskiInterval termin)
        {
            foreach (DayOfWeek danUNedelji in SlobodniDaniUNedelji)
            {
                if (danUNedelji == termin.Pocetak.DayOfWeek || danUNedelji == termin.Kraj.DayOfWeek)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TerminUGodisnjemOdmoruLekara(VremenskiInterval termin)
        {
            foreach (DateTime datum in SlobodniDani)
            {
                if (datum.Day == termin.Pocetak.Day || datum.Day == termin.Kraj.Day)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TerminUStandardnomRadnomVremenuLekara(VremenskiInterval termin)
        {
            int brojDanaRada = 0;
            if (!RadnoVremeLekaraUJednomDanu())
            {
                brojDanaRada = 1;   // tada lekar radi i sutradan
            }

            DateTime pocetakRadnogVremena = new DateTime(termin.Pocetak.Year, termin.Pocetak.Month, termin.Pocetak.Day,
                StandardnoRadnoVreme.Pocetak.Hour, StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
            DateTime krajRadnogVremena = new DateTime(termin.Pocetak.Year, termin.Pocetak.Month, termin.Pocetak.Day,
                StandardnoRadnoVreme.Kraj.Hour, StandardnoRadnoVreme.Kraj.Minute, 0, 0);
            krajRadnogVremena = krajRadnogVremena.AddDays(brojDanaRada);

            if (pocetakRadnogVremena <= termin.Pocetak && krajRadnogVremena >= termin.Kraj)
            {
                return true;
            }

            return false;
        }

        private bool TerminUVanrednomRadnomVremenuLekara(VremenskiInterval termin)
        {
            foreach (VremenskiInterval vanrednoRadnoVreme in VanrednaRadnaVremena)
            {
                if (vanrednoRadnoVreme.SadrziInterval(termin))
                {
                    return true;
                }
            }

            return false;
        }

        private bool DefinisanoVanrednoRadnoVremeLekaraZa(VremenskiInterval termin)
        {
            foreach (VremenskiInterval vanrednoRadnoVreme in VanrednaRadnaVremena)
            {
                if (vanrednoRadnoVreme.DaLiJeIstogDatuma(termin))
                {
                    return true;
                }
            }

            return false;
        }

        private bool RadnoVremeLekaraUJednomDanu()
        {
            // ukoliko lekaru radno vreme pocinje u jednom danu i traje preko tog dana
            int razlika = StandardnoRadnoVreme.Pocetak.Day - StandardnoRadnoVreme.Kraj.Day;
            if (razlika != 0)
            {
                // ako se radno vreme ne zavrsava istog dana tada on radi i u narednom danu
                return false;
            }

            return true;
        }
    }
}
