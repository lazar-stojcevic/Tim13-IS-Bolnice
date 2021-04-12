// File:    Lek.cs
// Author:  Matija
// Created: 31 March, 2021 16:28:40
// Purpose: Definition of Class Lek

using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Lek: INotifyPropertyChanged
{
   private String sifra;
   private String ime;
   private String opis;
   private List<String> alergeni;

    public Lek() {
        alergeni = new List<string>();
    }

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

    public List<String> Alergeni
    {
        get { return alergeni; }
        set
        {
            if (alergeni == value) return;
            alergeni = value;
            RaisePropertyChanged("alergeni");
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