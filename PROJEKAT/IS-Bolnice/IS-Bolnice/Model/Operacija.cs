// File:    Operacija.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Operacija

using System;

public class Operacija
{
   private DateTime vremePocetaOperacije;
   private DateTime vremeKrajaOperacije;
   
   public Soba soba;
   public Lekar lekar;

    public Operacija()
    {
        this.Pacijent = new Pacijent();
        this.Lekar = new Lekar();
        this.Soba = new Soba();
    }


    public Lekar Lekar
   {
      get
      {
         return lekar;
      }
      set
      {
         this.lekar = value;
      }
   }
   public Pacijent pacijent;
   
  
   public Pacijent Pacijent
   {
      get
      {
         return pacijent;
      }
      set
      {
         this.pacijent = value;
      }
   }

    public DateTime VremePocetaOperacije { get; set; }
    public DateTime VremeKrajaOperacije { get; set; }

    public Soba Soba { get; set; }

}