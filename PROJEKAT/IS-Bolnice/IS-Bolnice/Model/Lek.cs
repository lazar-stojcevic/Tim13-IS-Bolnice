// File:    Lek.cs
// Author:  Matija
// Created: 31 March, 2021 16:28:40
// Purpose: Definition of Class Lek

using System;
using System.Collections.Generic;
using System.ComponentModel;using IS_Bolnice.Model;

public class Lek: Entitet, INotifyPropertyChanged
{
   private String ime;
   private String opis;
   private List<Sastojak> sastojci;
   private List<Lek> zamenskiLekovi;
   private bool potrebanRecept;

    public Lek(): base("85236") {
        sastojci = new List<Sastojak>();
        zamenskiLekovi = new List<Lek>();
    }

    public Lek(string sifraLeka, string imeLeka, string opisLeka, List<Sastojak> sastojciLeka, List<Lek> zamenskiLekoviLeka, bool lekuPotrebanRecept): base(sifraLeka)
    {
        Id = sifraLeka;
        Ime = imeLeka;
        Opis = opisLeka;
        Alergeni = sastojciLeka;
        ZamenskiLekovi = zamenskiLekoviLeka;
        PotrebanRecept = lekuPotrebanRecept;
    }

    public Lek(string sifra): base(sifra)
    {
        Id = sifra;
    }

    public Lek(string sifraLeka, string imeLeka, string opisLeka, bool lekuPotrebanRecept) : base(sifraLeka)
    {
        Id = sifraLeka;
        Ime = imeLeka;
        Opis = opisLeka;
        Alergeni = new List<Sastojak>();
        ZamenskiLekovi = new List<Lek>();
        PotrebanRecept = lekuPotrebanRecept;
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