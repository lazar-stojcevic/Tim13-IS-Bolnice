// File:    Soba.cs
// Author:  Zola
// Created: Monday, March 22, 2021 6:39:01 PM
// Purpose: Definition of Class Soba

using System;

public class Soba
{

    public Soba(string id, bool zauzeta, bool podRenoviranje, RoomType tip, bool obrisano, int sprat, double kvadratura)
    {
        Id = id;
        Zauzeta = zauzeta;
        Tip = tip;
        Obrisano = obrisano;
        Sprat = sprat;
        Kvadratura = kvadratura;
    }

    public Soba()
    {

    }

    public Soba(string idSobe)
    {
        this.Id = idSobe;
    }

   
   public bool Izmeni(string noviID, int noviBrojSprata, RoomType noviTip, double novaKvadratura)
   {
        Id = noviID;
        Sprat = noviBrojSprata;
        Tip = noviTip;
        Kvadratura = novaKvadratura;
        return true;
    }
   
   public bool Obrisi()
   {
        this.Obrisano = true;
        return true;
    }
   
   public bool Zauzmi()
   {
        if (Zauzeta)
        {
            return false;
        }
        else
        {
            Zauzeta = true;
            return true;
        }
    }
   

    public bool Jednaka(Soba soba)
    {
        if (Id.Equals(soba.Id))
        {
            return true;
        }
        return false;
    }

    public string Id { get; set; }
    public bool Zauzeta { get; set; }
    public bool PodRenoviranje { get; set; }
    public RoomType Tip { get; set; }
    public bool Obrisano { get; set; }
    public int Sprat { get; set; }
    public double Kvadratura { get; set; }

}