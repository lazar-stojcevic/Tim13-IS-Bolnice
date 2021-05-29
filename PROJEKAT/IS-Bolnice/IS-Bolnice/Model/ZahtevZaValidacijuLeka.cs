// File:    ZahtevZaValidacijuLeka.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:24:36 PM
// Purpose: Definition of Class ZahtevZaValidacijuLeka

using System;
using System.Collections.Generic;
using IS_Bolnice.Model;

public class ZahtevZaValidacijuLeka : Entitet
{
   public Lek Lek { get; set; }
   public List<Lekar> lekariKomeIdeNaValidaciju { get; set;}

   public ZahtevZaValidacijuLeka(Lek lek):base(lek.Id)
    {
        lekariKomeIdeNaValidaciju = new List<Lekar>();
        Lek = lek;
    }

    public ZahtevZaValidacijuLeka(Lek noviLek, List<Lekar> lekari):base(noviLek.Id)
    {
        lekariKomeIdeNaValidaciju = lekari;
        Lek = noviLek;
    }
}