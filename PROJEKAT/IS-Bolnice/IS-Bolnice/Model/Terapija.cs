// File:    Terapija.cs
// Author:  Matija
// Created: 31 March, 2021 16:32:11
// Purpose: Definition of Class Terapija

using System;

public class Terapija
{
   private Double ucestanostKonzumiranja;
   private DateTime vremePocetka;
   private DateTime vremeKraja;
   
   private Lek lek;
   
   /// <summary>
   /// Property for Lek
   /// </summary>
   /// <pdGenerated>Default opposite class property</pdGenerated>
   public Lek Lek
   {
      get
      {
         return lek;
      }
      set
      {
         this.lek = value;
      }
   }

}