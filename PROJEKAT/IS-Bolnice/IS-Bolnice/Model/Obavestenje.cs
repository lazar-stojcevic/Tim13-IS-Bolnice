// File:    Obavestenje.cs
// Author:  Matija
// Created: 31 March, 2021 17:33:07
// Purpose: Definition of Class Obavestenje

using System;
using System.Collections.Generic;

public class Obavestenje
{
    public string Sifra { get; set; }
    public string Naslov { get; set; }
    public string Sadrzaj { get; set; }
    public DateTime VremeKreiranja { get; set; }
    public List<Uloge> Uloge { get; set; }
    public List<Pacijent> OdredjeniPacijenti { get; set; }
}