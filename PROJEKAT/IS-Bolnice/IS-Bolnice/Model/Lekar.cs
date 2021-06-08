using IS_Bolnice.Model;
using System;

public class Lekar : Korisnik
{
    public Lekar()
    {
        Id = Jmbg;
    }

    public Lekar(string jmbg)
    {
        Jmbg = jmbg;
        Id = jmbg;
    }

    public OblastLekara Oblast { get; set; }
    public Soba Ordinacija { get; set; }
    public RadnoVremeLekara RadnoVreme { get; set; }

    public bool JelLekarOpstePrakse()
    {
        return this.Oblast.Naziv.Equals(OblastLekara.oznakaOpstePrakse);
    }

    public bool TerminURadnomVremenuLekara(VremenskiInterval termin)
    {
        if (TerminUGodisnjemOdmoruLekara(termin))
        {
            return false;
        }

        if (TerminUVanrednomRadnomVremenu(termin))
        {
            return true;
        }

        if (TerminUSlobodnomDanuNedeljeLekara(termin))
        {
            return false;
        }

        if (TerminUStandardnomRadnomVremenu(termin) && !DefinisanoVanrednoRadnoVremeZa(termin))
        {
            return true;
        }

        return false;
    }

    private bool TerminUSlobodnomDanuNedeljeLekara(VremenskiInterval termin)
    {
        foreach (DayOfWeek danUNedelji in this.RadnoVreme.SlobodniDaniUNedelji)
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
        foreach (DateTime datum in this.RadnoVreme.SlobodniDani)
        {
            if (datum.Day == termin.Pocetak.Day || datum.Day == termin.Kraj.Day)
            {
                return true;
            }
        }

        return false;
    }

    private bool TerminUStandardnomRadnomVremenu(VremenskiInterval termin)
    {
        int brojDanaRada = 0;
        if (!RadnoVremeLekaraUJednomDanu())
        {
            brojDanaRada = 1;   // tada lekar radi i sutradan
        }

        DateTime pocetakRadnogVremena = new DateTime(termin.Pocetak.Year, termin.Pocetak.Month, termin.Pocetak.Day,
            this.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, this.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
        DateTime krajRadnogVremena = new DateTime(termin.Pocetak.Year, termin.Pocetak.Month, termin.Pocetak.Day,
            this.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, this.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);
        krajRadnogVremena = krajRadnogVremena.AddDays(brojDanaRada);

        if (pocetakRadnogVremena <= termin.Pocetak && krajRadnogVremena >= termin.Kraj)
        {
            return true;
        }

        return false;
    }

    private bool TerminUVanrednomRadnomVremenu(VremenskiInterval termin)
    {
        foreach (VremenskiInterval vanrednoRadnoVreme in this.RadnoVreme.VanrednaRadnaVremena)
        {
            if (vanrednoRadnoVreme.SadrziInterval(termin))
            {
                return true;
            }
        }

        return false;
    }

    private bool DefinisanoVanrednoRadnoVremeZa(VremenskiInterval termin)
    {
        foreach (VremenskiInterval vanrednoRadnoVreme in this.RadnoVreme.VanrednaRadnaVremena)
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
        int razlika = this.RadnoVreme.StandardnoRadnoVreme.Pocetak.Day - this.RadnoVreme.StandardnoRadnoVreme.Kraj.Day;
        if (razlika != 0)
        {
            // ako se radno vreme ne zavrsava istog dana tada on radi i u narednom danu
            return false;
        }

        return true;
    }

    public bool VecDodeljenoVanrednoRadnoVreme(VremenskiInterval interval)
    {
        foreach (VremenskiInterval vi in this.RadnoVreme.VanrednaRadnaVremena)
        {
            if (vi.DaLiSePreklapaSa(interval))
            {
                return true;
            }
        }

        return false;
    }
}