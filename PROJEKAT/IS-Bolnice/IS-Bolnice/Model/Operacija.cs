// File:    Operacija.cs
// Author:  Zola
// Created: Monday, March 22, 2021 5:52:52 PM
// Purpose: Definition of Class Operacija

using System;
using System.ComponentModel;
using IS_Bolnice.Model;

public class Operacija : Entitet, INotifyPropertyChanged
{

    private Pacijent pacijent;
    private Lekar lekar;
    private DateTime vremePocetkaOperacije;
    private DateTime vremeKrajaOperacije;
    private Soba soba;
    private Boolean hitna;

    public Operacija():base(Guid.NewGuid().ToString())
    {
        this.Pacijent = new Pacijent();
        this.Lekar = new Lekar();
        this.Soba = new Soba();
    }


    public Operacija(string idLekara, Pacijent pacijent, DateTime datum, OblastLekara oblast) : base(Guid.NewGuid().ToString())
    {
        Lekar = new Lekar();
        Pacijent = new Pacijent();
        Lekar.Jmbg = idLekara;
        Lekar.Oblast = oblast;
        Pacijent = pacijent;
        VremePocetkaOperacije = datum;
    }

    public Operacija(Operacija operacija) : base(operacija.Id)
    {
        Pacijent = operacija.Pacijent;
        Lekar = operacija.Lekar;
        VremePocetkaOperacije = operacija.VremePocetkaOperacije;
        VremeKrajaOperacije = operacija.VremeKrajaOperacije;
        Soba = operacija.Soba;
        Hitna = operacija.Hitna;
    }

    public Operacija(string id): base(id)
    {
        this.Pacijent = new Pacijent();
        this.Lekar = new Lekar();
        this.Soba = new Soba();
    }

    public Pacijent Pacijent
    {
        get { return pacijent; }
        set
        {
            if (pacijent == value) return;
            pacijent = value;
            RaisePropertyChanged("pacijent");
        }
    }

    public Lekar Lekar
    {
        get { return lekar; }
        set
        {
            if (lekar == value) return;
            lekar = value;
            RaisePropertyChanged("lekar");
        }
    }

    public DateTime VremePocetkaOperacije
    {
        get { return vremePocetkaOperacije; }
        set
        {
            if (vremePocetkaOperacije == value) return;
            vremePocetkaOperacije = value;
            RaisePropertyChanged("vremePocetkaOperacije");
        }
    }

    public DateTime VremeKrajaOperacije
    {
        get { return vremeKrajaOperacije; }
        set
        {
            if (vremeKrajaOperacije == value) return;
            vremeKrajaOperacije = value;
            RaisePropertyChanged("vremeKrajaOperacije");
        }
    }

    public Soba Soba
    {
        get { return soba; }
        set
        {
            if (soba == value) return;
            soba = value;
            RaisePropertyChanged("soba");
        }
    }

    public Boolean Hitna
    {
        get { return hitna; }
        set
        {
            if (hitna == value) return;
            hitna = value;
            RaisePropertyChanged("hitna");
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