// File:    Lekar.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Lekar

using IS_Bolnice.Model;
using System;

public class Lekar : Korisnik
{
    public Lekar() { }
    

    public OblastLekara Oblast { get; set; }
    public Soba Ordinacija { get; set; }
    public RadnoVremeLekara RadnoVreme { get; set; }

    public bool JelLekarOpstePrakse()
    {
        return this.Oblast.Naziv.Equals(OblastLekara.oznakaOpstePrakse);
    }

    public bool TerminURadnomVremenuLekara(VremenskiInterval termin)
    {
        if (TerminUSlobodnomDanuLekara(termin))
        {
            return false;
        }

        if (TerminUStandardnomRadnomVremenu(termin))
        {
            return true;
        }

        if (TerminUVanrednomRadnomVremenu(termin))
        {
            return true;
        }

        return false;
    }

    private bool TerminUSlobodnomDanuLekara(VremenskiInterval termin)
    {
        foreach (DateTime datum in this.RadnoVreme.SlobodniDani)
        {
            if (datum.Day == termin.Pocetak.Day || datum.Day == termin.Kraj.Day)
            {
                return true;
            }
        }

        foreach (DayOfWeek danUNedelji in this.RadnoVreme.SlobodniDaniUNedelji)
        {
            if (danUNedelji == termin.Pocetak.DayOfWeek || danUNedelji == termin.Kraj.DayOfWeek)
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
        DateTime krajRadnogVremena = new DateTime(termin.Pocetak.Year, termin.Pocetak.Month, termin.Pocetak.Day + brojDanaRada,
            this.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, this.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);

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
}