// File:    Predmet.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Predmet

using System;
using IS_Bolnice.Model;

public class Predmet : Entitet
{
    public Predmet(string id):base(id)
    {}
  
    public TipOpreme Tip { get; set; }
    public int Kolicina { get; set; }
    public string Naziv { get; set; }
   // public string Id { get; set; }

    public bool Obrisano { get; set; }
}