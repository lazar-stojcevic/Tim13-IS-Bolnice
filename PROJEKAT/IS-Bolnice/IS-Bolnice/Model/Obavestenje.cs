// File:    Obavestenje.cs
// Author:  Matija
// Created: 31 March, 2021 17:33:07
// Purpose: Definition of Class Obavestenje

using System;
using System.Collections.Generic;

namespace IS_Bolnice.Model
{
    public class Obavestenje
    {
        public string Id { get; set; }
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public DateTime VremeKreiranja { get; set; }
        public List<Uloge> Uloge { get; set; }
        public List<Pacijent> OdredjeniPacijenti { get; set; }

        public Obavestenje()
        {
            Uloge = new List<Uloge>();
            OdredjeniPacijenti = new List<Pacijent>();
        }
    }
}