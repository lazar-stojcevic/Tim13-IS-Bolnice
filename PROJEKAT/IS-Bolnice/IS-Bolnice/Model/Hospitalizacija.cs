// File:    Hospitalizacija.cs
// Author:  Zola
// Created: Monday, May 17, 2021 7:16:29 PM
// Purpose: Definition of Class Hospitalizacija

using System;
using System.ComponentModel;
using IS_Bolnice.Model;

public class Hospitalizacija :Entitet ,INotifyPropertyChanged
{
    private DateTime pocetakHospitalizacije;
    private DateTime krajHospitalizacije;

    private Soba soba;

    private Pacijent pacijent;

    public event PropertyChangedEventHandler PropertyChanged;

    public Hospitalizacija(string idPacijenta, string idSobe, DateTime krajHospitalizacije):base(idPacijenta + "+" + idSobe)
    {
        PocetakHospitalizacije = DateTime.Now;
        KrajHospitalizacije = krajHospitalizacije;

        Soba = new Soba();
        Soba.Id = idSobe;

        Pacijent = new Pacijent();
        Pacijent.Jmbg = idPacijenta;
    }

    public Hospitalizacija():base("")
    {
        Soba = new Soba();
        Pacijent = new Pacijent();
    }

    /// <summary>
    /// Property for Soba
    /// </summary>
    /// <pdGenerated>Default opposite class property</pdGenerated>

    public DateTime PocetakHospitalizacije
    {
        get
        {
            return pocetakHospitalizacije;
        }
        set
        {
            if (pocetakHospitalizacije == value) return;
            pocetakHospitalizacije = value;
            RaisePropertyChanged("pocetakHospitalizacije");
        }
    }

    public DateTime KrajHospitalizacije
    {
        get
        {
            return krajHospitalizacije;
        }
        set
        {
            if (krajHospitalizacije == value) return;
            krajHospitalizacije = value;
            RaisePropertyChanged("krajHospitalizacije");
        }
    }

    public Soba Soba
   {
      get
      {
         return soba;
      }
      set
      {
          if (soba == value) return;
          soba = value;
          RaisePropertyChanged("soba");
        }
   }
   public Pacijent Pacijent
   {
       get
       {
           return pacijent;
       }
       set
       {
           if (pacijent == value) return;
           pacijent = value;
           RaisePropertyChanged("pacijent");
        }
   }

   private void RaisePropertyChanged(string propName)
   {
       PropertyChangedEventHandler eh = PropertyChanged;
       if (eh != null)
       {
           eh(this, new PropertyChangedEventArgs(propName));
       }
   }
}