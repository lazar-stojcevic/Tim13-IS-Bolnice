// File:    Pacijent.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Pacijent

using System;

public class Pacijent : Korisnik
{
    private Lekar izabraniLekar;
    private bool guest = false;

    public Lekar IzabraniLekar { get; set; }
    public bool Guest { get; set; }
}