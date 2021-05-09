// File:    Renovacija.cs
// Author:  teddy
// Created: Monday, April 26, 2021 3:39:18 PM
// Purpose: Definition of Class Renovacija

using System;

public class Renovacija
{
    public DateTime DatumPocetka { get; set; }
    public DateTime DatumKraja { get; set; }

    public Soba ProstorijaZaRenoviranje { get; set; }

    public Renovacija() { }

    public Renovacija(DateTime datumPocetak, DateTime datumKraja, Soba prostorija) {
        DatumPocetka = datumPocetak;
        DatumKraja = datumKraja;
        ProstorijaZaRenoviranje = prostorija;
    }

}