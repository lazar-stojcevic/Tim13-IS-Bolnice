// File:    Predmet.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Predmet

using System;

public class Predmet
{
  
    public TipOpreme Tip { get; set; }
    public int Kolicina { get; set; }
    public string Naziv { get; set; }
    public string Id { get; set; }

    public bool Obrisano { get; set; }
}