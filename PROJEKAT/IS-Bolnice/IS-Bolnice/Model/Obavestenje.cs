// File:    Obavestenje.cs
// Author:  Matija
// Created: 31 March, 2021 17:33:07
// Purpose: Definition of Class Obavestenje

using System;

public class Obavestenje
{
    private string sifra;
    private string naslov;
    private string sadrzaj;
    private DateTime vremeKreiranja;

    public string Sifra { get; set; }
    public string Naslov { get; set; }
    public string Sadrzaj { get; set; }
    public DateTime VremeKreiranja { get; set; }
}