using IS_Bolnice.Model;
using System;

public class Renovacija: Entitet
{
    public DateTime DatumPocetka { get; set; }
    public DateTime DatumKraja { get; set; }

    public Soba ProstorijaZaRenoviranje { get; set; }

    public Renovacija(): base(Guid.NewGuid().ToString()) { }

    public Renovacija(DateTime datumPocetak, DateTime datumKraja, Soba prostorija): base(Guid.NewGuid().ToString()) {
        DatumPocetka = datumPocetak;
        DatumKraja = datumKraja;
        ProstorijaZaRenoviranje = prostorija;
    }

}