using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class SastojakFajlRepozitorijum : GenerickiFajlRepozitorijum<Sastojak>, ISastojakRepozitorijum
{

    public SastojakFajlRepozitorijum() : base(@"..\..\Datoteke\sastojci.txt")
    {
    }
   
    public override Sastojak KreirajEntitet(string[] podaciEntiteta)
    {
        Sastojak sastojak = new Sastojak(podaciEntiteta[0]);
        return sastojak;
    }

    public override string KreirajTextZaUpis(Sastojak entitet)
    {
        return entitet.Ime;
    }
}