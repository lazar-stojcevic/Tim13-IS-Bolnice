using System;
using System.Collections.Generic;

public class Bolnica
{
   private string ime;
   private string adresa;
   private string eMail;
   private string brojTelefona;
   
   public System.Collections.Generic.List<Soba> soba;

    public Bolnica(string ime, string adresa, string eMail, string brTel, List<Soba> sobe)
    {
        this.ime = ime;
        this.adresa = adresa;
        this.eMail = eMail;
        this.brojTelefona = brTel;
        this.soba = sobe;
    }

    public Bolnica()
    {

    }


    public System.Collections.Generic.List<Soba> Soba
   {
      get
      {
         if (soba == null)
            soba = new System.Collections.Generic.List<Soba>();
         return soba;
      }
      set
      {
         RemoveAllSoba();
         if (value != null)
         {
            foreach (Soba oSoba in value)
               AddSoba(oSoba);
         }
      }
   }

   public void AddSoba(Soba newSoba)
   {
      if (newSoba == null)
         return;
      if (this.soba == null)
         this.soba = new System.Collections.Generic.List<Soba>();
      if (!this.soba.Contains(newSoba))
         this.soba.Add(newSoba);
   }
   
   public void RemoveSoba(Soba oldSoba)
   {
      if (oldSoba == null)
         return;
      if (this.soba != null)
         if (this.soba.Contains(oldSoba))
            this.soba.Remove(oldSoba);
   }
   
   public void RemoveAllSoba()
   {
      if (soba != null)
         soba.Clear();
   }
   public Upravnik upravnik;

    public string Ime { get; set; }
    public string Adresa { get; set; }
    public string EMail { get; set; }
    public string BrojTelefona { get; set; }

}