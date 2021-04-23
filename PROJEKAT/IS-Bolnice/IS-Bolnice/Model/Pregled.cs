// File:    Pregled.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pregled

using System;

public class Pregled
{
    public Pregled() { }

    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }
    public DateTime VremePocetkaPregleda { get; set; }
    public DateTime VremeKrajaPregleda { get; set; }
}