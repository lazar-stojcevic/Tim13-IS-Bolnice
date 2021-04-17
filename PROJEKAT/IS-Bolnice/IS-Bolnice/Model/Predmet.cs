// File:    Predmet.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Predmet

using System;

public class Predmet
{
   private TipOpreme tip;
   private int kolicina;
   private string naziv;
   private string id;
   private bool obrisano;
   
   public Soba soba;
   
   /// <summary>
   /// Property for Soba
   /// </summary>
   /// <pdGenerated>Default opposite class property</pdGenerated>
   public Soba Soba
   {
      get
      {
         return soba;
      }
      set
      {
         if (this.soba == null || !this.soba.Equals(value))
         {
            if (this.soba != null)
            {
               Soba oldSoba = this.soba;
               this.soba = null;
               oldSoba.RemovePredmet(this);
            }
            if (value != null)
            {
               this.soba = value;
               this.soba.AddPredmet(this);
            }
         }
      }
   }
    public TipOpreme Tip { get; set; }
    public int Kolicina { get; set; }
    public string Naziv { get; set; }
    public string Id { get; set; }

    public bool Obrisano { get; set; }
}