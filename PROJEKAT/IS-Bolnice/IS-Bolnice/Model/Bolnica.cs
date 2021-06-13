using IS_Bolnice.Model;
using System.Collections.Generic;

public class Bolnica: Entitet
{

    public Bolnica(string ime, string adresa, string eMail, string brTel, List<Soba> sobe): base(ime)
    {
        this.Ime = ime;
        this.Adresa = adresa;
        this.EMail = eMail;
        this.BrojTelefona = brTel;
        this.Soba = sobe;
    }

    public Bolnica(): base("1")
    {

    }


    public System.Collections.Generic.List<Soba> Soba
   {
      get
      {
         if (Sobe == null)
                Sobe = new System.Collections.Generic.List<Soba>();
         return Sobe;
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
      if (this.Sobe == null)
         this.Sobe = new System.Collections.Generic.List<Soba>();
      if (!this.Sobe.Contains(newSoba))
         this.Sobe.Add(newSoba);
   }
   
   public void RemoveSoba(Soba oldSoba)
   {
      if (oldSoba == null)
         return;
      if (this.Sobe != null)
         if (this.Sobe.Contains(oldSoba))
            this.Sobe.Remove(oldSoba);
   }
   
   public void RemoveAllSoba()
   {
      if (Sobe != null)
            Sobe.Clear();
   }
   public Upravnik upravnik;

    public string Ime { get; set; }
    public string Adresa { get; set; }
    public string EMail { get; set; }
    public string BrojTelefona { get; set; }

    public List<Soba> Sobe { get; set; }

}