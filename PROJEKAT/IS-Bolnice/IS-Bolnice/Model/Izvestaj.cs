// File:    Izvestaj.cs
// Author:  Matija
// Created: 31 March, 2021 16:29:54
// Purpose: Definition of Class Izvestaj

using System;

public class Izvestaj
{
    public string Opis { get; set; }
    public DateTime DatumKreiranja { get; set; }
    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }

    public Izvestaj() { }

    public Izvestaj(Lekar lekar, Pacijent pacijent, string opis, DateTime datumKreiranja)
    {
        this.Lekar = lekar;
        this.Pacijent = pacijent;
        this.Opis = opis;
        this.DatumKreiranja = datumKreiranja;
    }


    private System.Collections.Generic.List<Terapija> terapija;

    public System.Collections.Generic.List<Terapija> Terapija
    {
        get
        {
            if (terapija == null)
                terapija = new System.Collections.Generic.List<Terapija>();
            return terapija;
        }
        set
        {
            RemoveAllTerapija();
            if (value != null)
            {
                foreach (Terapija oTerapija in value)
                    AddTerapija(oTerapija);
            }
        }
    }

    public void AddTerapija(Terapija newTerapija)
    {
        if (newTerapija == null)
            return;
        if (this.terapija == null)
            this.terapija = new System.Collections.Generic.List<Terapija>();
        if (!this.terapija.Contains(newTerapija))
            this.terapija.Add(newTerapija);
    }

    public void RemoveTerapija(Terapija oldTerapija)
    {
        if (oldTerapija == null)
            return;
        if (this.terapija != null)
            if (this.terapija.Contains(oldTerapija))
                this.terapija.Remove(oldTerapija);
    }

    public void RemoveAllTerapija()
    {
        if (terapija != null)
            terapija.Clear();
    }

}