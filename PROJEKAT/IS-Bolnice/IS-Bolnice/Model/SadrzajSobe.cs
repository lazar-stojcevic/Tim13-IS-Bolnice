// File:    SadrzajSobe.cs
// Author:  Matija
// Created: Friday, April 23, 2021 3:30:43 PM
// Purpose: Definition of Class SadrzajSobe

using System;

public class SadrzajSobe
{

    public int Kolicina { get; set; }
    public DateTime DatumPremestanja { get; set; }
    public Soba NovaSoba { get; set; }
    public Soba Soba { get; set; }
    public Predmet Predmet { get; set; }

    public SadrzajSobe(string sobaID, string predmetID, int kolicina) {

        Kolicina = kolicina;
        Soba = new Soba() ;
        Soba.Id = sobaID;
        Predmet = new Predmet();
        Predmet.Id = predmetID;
    }

    public SadrzajSobe(Soba soba, Predmet predmet, int kolicina, DateTime datumPremestanja, Soba novaSoba)
    {

        Kolicina = kolicina;
        Soba = soba;
        Predmet = predmet;
        DatumPremestanja = datumPremestanja;
        NovaSoba = novaSoba;
    }

    public SadrzajSobe() { }

}