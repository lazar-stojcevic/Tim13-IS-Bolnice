// File:    ZahtevZaValidacijuLeka.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:24:36 PM
// Purpose: Definition of Class ZahtevZaValidacijuLeka

using System;
using System.Collections.Generic;

public class ZahtevZaValidacijuLeka
{
   public Lek Lek { get; set; }
   public List<Lekar> lekariKomeIdeNaValidaciju { get; set;}

   public ZahtevZaValidacijuLeka()
    {
        lekariKomeIdeNaValidaciju = new List<Lekar>();
    }

}