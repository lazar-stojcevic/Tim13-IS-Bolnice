// File:    Lekar.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Lekar

using System;

public class Lekar : Korisnik
{
    private TipLekara tip;

    public Soba soba;

    public Lekar(TipLekara tip, Soba soba) : base()
    {
        this.tip = tip;
        this.soba = soba;
    }

    public Lekar()
    {
    }

    public TipLekara Tip { get; set; }

}