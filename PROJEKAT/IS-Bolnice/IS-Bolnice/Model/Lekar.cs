// File:    Lekar.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Lekar

using System;

public class Lekar : Korisnik
{
    public Lekar() { }

    public TipLekara Tip { get; set; }
    public Soba Ordinacija { get; set; }
    public DateTime PocetakRadnogVremena { get; set; }
    public DateTime KrajRadnogVremena { get; set; }
}