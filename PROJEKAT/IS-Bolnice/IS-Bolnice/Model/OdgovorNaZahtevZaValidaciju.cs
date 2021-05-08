// File:    OdgovorNaZahtevZaValidaciju.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:29:25 PM
// Purpose: Definition of Class OdgovorNaZahtevZaValidaciju

using System;

public class OdgovorNaZahtevZaValidaciju
{
    public string Obrazlozenje { get; set; }

    public Lek Lek { get; set; }

    public OdgovorNaZahtevZaValidaciju() { }

    public OdgovorNaZahtevZaValidaciju(Lek lek, string obrazlozenje)
    {
        Lek = lek;
        Obrazlozenje = obrazlozenje;
    }

}