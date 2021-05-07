// File:    Operacija.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Operacija

using System;

public class Operacija
{
    public Operacija()
    {
        this.Pacijent = new Pacijent();
        this.Lekar = new Lekar();
        this.Soba = new Soba();
    }

    public Operacija(Pacijent pacijent, Lekar lekar, DateTime vremePocetkaOperacije, DateTime vremeKrajaOperacije, Soba soba, Boolean hitna)
    {
        Pacijent = pacijent;
        Lekar = lekar;
        VremePocetkaOperacije = vremePocetkaOperacije;
        VremeKrajaOperacije = vremeKrajaOperacije;
        Soba = soba;
        Hitna = hitna;
    }

    public Operacija(Operacija operacija)
    {
        Pacijent = operacija.Pacijent;
        Lekar = operacija.Lekar;
        VremePocetkaOperacije = operacija.VremePocetkaOperacije;
        VremeKrajaOperacije = operacija.VremeKrajaOperacije;
        Soba = operacija.Soba;
        Hitna = operacija.Hitna;
    }

    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; } 
    public DateTime VremePocetkaOperacije { get; set; }
    public DateTime VremeKrajaOperacije { get; set; }
    public Soba Soba { get; set; }
    public Boolean Hitna { get; set; }

}