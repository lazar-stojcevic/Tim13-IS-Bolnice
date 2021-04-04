// File:    Pregled.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pregled

using System;

public class Pregled
{
   private DateTime vremePocetkaPregleda;
   private DateTime vremeKrajaPregleda;
   private Pacijent pacijent;
   private Lekar lekar;

    public Pregled(DateTime vremePocetkaPregleda, DateTime vremeKrajaPregleda, Pacijent pacijent, Lekar lekar)
    {
        this.vremePocetkaPregleda = vremePocetkaPregleda;
        this.vremeKrajaPregleda = vremeKrajaPregleda;
        this.pacijent = pacijent;
        this.lekar = lekar;
    }

    public Pregled() { }

    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }
    public DateTime VremePocetkaPregleda { get; set; }
    public DateTime VremeKrajaPregleda { get; set; }

}