// File:    Pregled.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pregled

using System;

public class Pregled
{
   private DateTime vremePocetkaPregleda;
   private DateTime vremeKrajaPregleda;

    public Pregled(DateTime vremePocetkaPregleda, DateTime vremeKrajaPregleda, Pacijent pacijent, Lekar lekar)
    {
        this.vremePocetkaPregleda = vremePocetkaPregleda;
        this.vremeKrajaPregleda = vremeKrajaPregleda;
        this.pacijent = pacijent;
        this.lekar = lekar;
    }

    public Pacijent pacijent;
   public Lekar lekar;

}