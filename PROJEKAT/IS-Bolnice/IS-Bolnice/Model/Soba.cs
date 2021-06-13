// File:    Soba.cs
// Author:  Zola
// Created: Monday, March 22, 2021 6:39:01 PM
// Purpose: Definition of Class Soba

using IS_Bolnice.Model;
using IS_Bolnice.StanjeSobe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

public class Soba: Entitet
{

    public Soba(string id, RoomType tip, bool obrisano, int sprat, double kvadratura): base(id)
    {
        Id = id;
        Tip = tip;
        Obrisano = obrisano;
        Sprat = sprat;
        Kvadratura = kvadratura;
    }

    public Soba(): base("101")
    {

    }

    public Soba(string idSobe)
    {
        this.Id = idSobe;
    }
   

    public bool Jednaka(Soba soba)
    {
        if (Id.Equals(soba.Id))
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return Id + " " + Kvadratura + "m^2 " + "Sprat: " + Sprat;
    }

    public RoomType Tip { get; set; }
    public bool Obrisano { get; set; }
    public int Sprat { get; set; }
    public double Kvadratura { get; set; }
    public IStanjeSobe StanjeSobe { get; set; }

}