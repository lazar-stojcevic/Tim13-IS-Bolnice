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
   private List<Sastojak> sastojci;
   private List<Lek> zamenskiLekovi;
   private bool potrebanRecept;

    public Lek() {
        sastojci = new List<Sastojak>();
        zamenskiLekovi = new List<Lek>();
    }

    public Lek(string sifraLeka, string imeLeka, string opisLeka, List<Sastojak> sastojciLeka, List<Lek> zamenskiLekoviLeka, bool lekuPotrebanRecept)
    {
        Sifra = sifraLeka;
        Ime = imeLeka;
        Opis = opisLeka;
        Alergeni = sastojciLeka;
        ZamenskiLekovi = zamenskiLekoviLeka;
        PotrebanRecept = lekuPotrebanRecept;
    }

    public Lek(string sifra)
    {
        Sifra = sifra;
    }

    public Lek(string sifraLeka, string imeLeka, string opisLeka, bool lekuPotrebanRecept)
    {
        Sifra = sifraLeka;
        Ime = imeLeka;
        Opis = opisLeka;
        Alergeni = new List<Sastojak>();
        ZamenskiLekovi = new List<Lek>();
        PotrebanRecept = lekuPotrebanRecept;
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

    public List<Lek> ZamenskiLekovi
    {
        get { return zamenskiLekovi; }
        set
        {
            if (zamenskiLekovi == value) return;
            zamenskiLekovi = value;
            RaisePropertyChanged("zamenskiLekovi");
        }
    }

    public List<Sastojak> Alergeni
    {
        get { return sastojci; }
        set
        {
            if (sastojci == value) return;
            sastojci = value;
            RaisePropertyChanged("sastojci");
        }
    }

    public bool PotrebanRecept
    {
        get { return potrebanRecept; }
        set
        {
            if (potrebanRecept == value) return;
            potrebanRecept = value;
            RaisePropertyChanged("potrebanRecept");
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