// File:    Terapija.cs
// Author:  Matija
// Created: 31 March, 2021 16:32:11
// Purpose: Definition of Class Terapija

using System;
using System.ComponentModel;

public class Terapija : INotifyPropertyChanged
{
   private Double ucestanostKonzumiranja;
   private DateTime vremePocetka;
   private DateTime vremeKraja;
   
   private Lek lek;

    public Double UcestanostKonzumiranja
    {
        get { return ucestanostKonzumiranja; }
        set
        {
            if (ucestanostKonzumiranja == value) return;
            ucestanostKonzumiranja = value;
            RaisePropertyChanged("ucestanostKonzumiranja");
        }
    }

    public DateTime VremePocetka
    {
        get { return vremePocetka; }
        set
        {
            if (vremePocetka == value) return;
            vremePocetka = value;
            RaisePropertyChanged("vremePocetka");
        }
    }

    public DateTime VremeKraja
    {
        get { return vremePocetka; }
        set
        {
            if (vremeKraja == value) return;
            vremeKraja = value;
            RaisePropertyChanged("vremeKraja");
        }
    }

    public Lek Lek
    {
        get { return lek; }
        set
        {
            if (lek == value) return;
            lek = value;
            RaisePropertyChanged("lek");
        }
    }

   

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propName)
    {
        PropertyChangedEventHandler eh = PropertyChanged;
        if (eh != null)
        {
            eh(this, new PropertyChangedEventArgs(propName));
        }
    }
}