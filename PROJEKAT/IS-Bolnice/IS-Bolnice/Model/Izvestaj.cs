// File:    Izvestaj.cs
// Author:  Matija
// Created: 31 March, 2021 16:29:54
// Purpose: Definition of Class Izvestaj

using System;

public class Izvestaj
{
   private String opis;
   private DateTime datum;
   
   private System.Collections.Generic.List<Terapija> terapija;
   
   /// <summary>
   /// Property for collection of Terapija
   /// </summary>
   /// <pdGenerated>Default opposite class collection property</pdGenerated>
   public System.Collections.Generic.List<Terapija> Terapija
   {
      get
      {
         if (terapija == null)
            terapija = new System.Collections.Generic.List<Terapija>();
         return terapija;
      }
      set
      {
         RemoveAllTerapija();
         if (value != null)
         {
            foreach (Terapija oTerapija in value)
               AddTerapija(oTerapija);
         }
      }
   }
   
   /// <summary>
   /// Add a new Terapija in the collection
   /// </summary>
   /// <pdGenerated>Default Add</pdGenerated>
   public void AddTerapija(Terapija newTerapija)
   {
      if (newTerapija == null)
         return;
      if (this.terapija == null)
         this.terapija = new System.Collections.Generic.List<Terapija>();
      if (!this.terapija.Contains(newTerapija))
         this.terapija.Add(newTerapija);
   }
   
   /// <summary>
   /// Remove an existing Terapija from the collection
   /// </summary>
   /// <pdGenerated>Default Remove</pdGenerated>
   public void RemoveTerapija(Terapija oldTerapija)
   {
      if (oldTerapija == null)
         return;
      if (this.terapija != null)
         if (this.terapija.Contains(oldTerapija))
            this.terapija.Remove(oldTerapija);
   }
   
   /// <summary>
   /// Remove all instances of Terapija from the collection
   /// </summary>
   /// <pdGenerated>Default removeAll</pdGenerated>
   public void RemoveAllTerapija()
   {
      if (terapija != null)
         terapija.Clear();
   }
   
   public Pacijent pacijent;

}