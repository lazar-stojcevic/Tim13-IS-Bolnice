using System;
using System.Collections.Generic;
using IS_Bolnice.Model;

public class Izvestaj : Entitet
{
    public string Opis { get; set; }
    public DateTime DatumKreiranja { get; set; }
    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }

    public Izvestaj():base("") { }

    public Izvestaj(Lekar lekar, Pacijent pacijent, string opis, DateTime datumKreiranja):base(Guid.NewGuid().ToString())
    {
        this.Lekar = lekar;
        this.Pacijent = pacijent;
        this.Opis = opis;
        this.DatumKreiranja = datumKreiranja;
        this.Terapija = new List<Terapija>();
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

    public void RemoveAllTerapija()
    {
        if (terapija != null)
            terapija.Clear();
    }

}