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

    public Lekar Lekar { get; set; }
    public Pacijent Pacijent { get; set; }
    public DateTime VremePocetaOperacije { get; set; }
    public DateTime VremeKrajaOperacije { get; set; }
    public Soba Soba { get; set; }

}