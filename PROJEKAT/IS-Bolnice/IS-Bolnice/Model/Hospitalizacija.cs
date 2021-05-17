// File:    Hospitalizacija.cs
// Author:  Zola
// Created: Monday, May 17, 2021 7:16:29 PM
// Purpose: Definition of Class Hospitalizacija

using System;

public class Hospitalizacija
{
   private DateTime pocetakHospitalizacije;
   private DateTime krajHospitalizacije;
   
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
         this.soba = value;
      }
   }
   public Pacijent pacijent;

}