// File:    Pregled.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pregled

using System;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

public class Pregled
{
    public Pregled()
    {
        Lekar = new Lekar();
        Pacijent = new Pacijent();
    }

    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }
    public DateTime VremePocetkaPregleda { get; set; }
    public DateTime VremeKrajaPregleda { get; set; }

    public Pregled(Pacijent pacijent, Lekar lekar, DateTime vremePocetkaPregleda, DateTime vremeKrajaPregleda)
    {
        Pacijent = pacijent;
        Lekar = lekar;
        VremePocetkaPregleda = vremePocetkaPregleda;
        VremeKrajaPregleda = vremeKrajaPregleda;
    }

    public Pregled(Pregled pregled)
    {
        Pacijent = pregled.Pacijent;
        Lekar = pregled.Lekar;
        VremePocetkaPregleda = pregled.VremePocetkaPregleda;
        VremeKrajaPregleda = pregled.VremeKrajaPregleda;
    }

    public Pregled(string idLekara, Pacijent pacijent, DateTime datum, OblastLekara oblast)
    {
        Lekar = new Lekar();
        Pacijent = new Pacijent();
        Lekar.Jmbg = idLekara;
        Lekar.Oblast = oblast;
        Pacijent = pacijent;
        VremePocetkaPregleda = datum;
    }
}