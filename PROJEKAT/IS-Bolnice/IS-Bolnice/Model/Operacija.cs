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
   
   /// <summary>
   /// Property for Lekar
   /// </summary>
   /// <pdGenerated>Default opposite class property</pdGenerated>
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
   
   /// <summary>
   /// Property for Pacijent
   /// </summary>
   /// <pdGenerated>Default opposite class property</pdGenerated>
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

}