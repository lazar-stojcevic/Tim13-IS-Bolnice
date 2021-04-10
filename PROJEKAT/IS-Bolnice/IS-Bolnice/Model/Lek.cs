// File:    Lek.cs
// Author:  Matija
// Created: 31 March, 2021 16:28:40
// Purpose: Definition of Class Lek

using System;
using System.ComponentModel;

public class Lek: INotifyPropertyChanged
{
   private String sifra;
   private String ime;
   private String opis;

    public String Sifra
    {
        get { return sifra; }
        set
        {
            if (sifra == value) return;
            sifra = value;
            RaisePropertyChanged("sifra");
        }
    }

    public String Ime
    {
        get { return ime; }
        set
        {
            if (ime == value) return;
            ime = value;
            RaisePropertyChanged("ime");
        }
    }

    public String Opis
    {
        get { return opis; }
        set
        {
            if (opis == value) return;
            opis = value;
            RaisePropertyChanged("opis");
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