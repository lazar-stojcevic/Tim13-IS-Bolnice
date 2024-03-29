using System;
using IS_Bolnice.Model;

public class Pregled: Entitet
{
    public Pregled(): base(Guid.NewGuid().ToString())
    {
        Lekar = new Lekar();
        Pacijent = new Pacijent();
    }

    public Pacijent Pacijent { get; set; }
    public Lekar Lekar { get; set; }
    public DateTime VremePocetkaPregleda { get; set; }
    public DateTime VremeKrajaPregleda { get; set; }

    public Pregled(Pacijent pacijent, Lekar lekar, DateTime vremePocetkaPregleda, DateTime vremeKrajaPregleda): base(Guid.NewGuid().ToString())
    {
        Pacijent = pacijent;
        Lekar = lekar;
        VremePocetkaPregleda = vremePocetkaPregleda;
        VremeKrajaPregleda = vremeKrajaPregleda;
    }

    public Pregled(Pregled pregled) : base(pregled.Id)
    {
        Pacijent = pregled.Pacijent;
        Lekar = pregled.Lekar;
        VremePocetkaPregleda = pregled.VremePocetkaPregleda;
        VremeKrajaPregleda = pregled.VremeKrajaPregleda;
    }

    public Pregled(string idLekara, Pacijent pacijent, DateTime datum, OblastLekara oblast) : base(Guid.NewGuid().ToString())
    {
        Lekar = new Lekar();
        Pacijent = new Pacijent();
        Lekar.Jmbg = idLekara;
        Lekar.Oblast = oblast;
        Pacijent = pacijent;
        VremePocetkaPregleda = datum;
    }
}