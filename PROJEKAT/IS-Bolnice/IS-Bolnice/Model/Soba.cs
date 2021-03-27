// File:    Soba.cs
// Author:  Zola
// Created: Monday, March 22, 2021 6:39:01 PM
// Purpose: Definition of Class Soba

using System;

public class Soba
{
   private string id;
   private bool zauzeta = false;
   private bool podRenoviranje = false;
   private RoomType tip;
   private bool obrisano = false;
   private int sprat;
   private double kvadratura;

   public Soba(string id, bool zauzeta, bool podRenoviranje, RoomType tip, bool obrisano, int sprat, double kvadratura)
    {
        this.id = id;
        this.zauzeta = zauzeta;
        this.podRenoviranje = podRenoviranje;
        this.tip = tip;
        this.obrisano = obrisano;
        this.sprat = sprat;
        this.kvadratura = kvadratura;
    }

   public Soba()
    {

    }

   public bool Renoviraj()
   {
        this.podRenoviranje = true;
        return true;
    }
   
   public bool Izmeni(string noviID, string noviBrojSprata, bool noviStatusZauzetosti, bool noviStatusZaRenoviranja)
   {
      throw new NotImplementedException();
   }
   
   public bool Obrisi()
   {
      throw new NotImplementedException();
   }
   
   public bool Zauzmi()
   {
      throw new NotImplementedException();
   }
   
   public System.Collections.Generic.List<Predmet> predmet;
   
   /// <summary>
   /// Property for collection of Predmet
   /// </summary>
   /// <pdGenerated>Default opposite class collection property</pdGenerated>
   public System.Collections.Generic.List<Predmet> Predmet
   {
      get
      {
         if (predmet == null)
            predmet = new System.Collections.Generic.List<Predmet>();
         return predmet;
      }
      set
      {
         RemoveAllPredmet();
         if (value != null)
         {
            foreach (Predmet oPredmet in value)
               AddPredmet(oPredmet);
         }
      }
   }
   
   /// <summary>
   /// Add a new Predmet in the collection
   /// </summary>
   /// <pdGenerated>Default Add</pdGenerated>
   public void AddPredmet(Predmet newPredmet)
   {
      if (newPredmet == null)
         return;
      if (this.predmet == null)
         this.predmet = new System.Collections.Generic.List<Predmet>();
      if (!this.predmet.Contains(newPredmet))
      {
         this.predmet.Add(newPredmet);
         newPredmet.Soba = this;
      }
   }
   
   /// <summary>
   /// Remove an existing Predmet from the collection
   /// </summary>
   /// <pdGenerated>Default Remove</pdGenerated>
   public void RemovePredmet(Predmet oldPredmet)
   {
      if (oldPredmet == null)
         return;
      if (this.predmet != null)
         if (this.predmet.Contains(oldPredmet))
         {
            this.predmet.Remove(oldPredmet);
            oldPredmet.Soba = null;
         }
   }
   
   /// <summary>
   /// Remove all instances of Predmet from the collection
   /// </summary>
   /// <pdGenerated>Default removeAll</pdGenerated>
   public void RemoveAllPredmet()
   {
      if (predmet != null)
      {
         System.Collections.ArrayList tmpPredmet = new System.Collections.ArrayList();
         foreach (Predmet oldPredmet in predmet)
            tmpPredmet.Add(oldPredmet);
         predmet.Clear();
         foreach (Predmet oldPredmet in tmpPredmet)
            oldPredmet.Soba = null;
         tmpPredmet.Clear();
      }
   }

    public string Id { get; set; }
    public bool Zauzeta { get; set; }
    public bool PodRenoviranje { get; set; }
    public RoomType Tip { get; set; }
    public bool Obrisano { get; set; }
    public int Sprat { get; set; }
    public double Kvadratura { get; set; }

}