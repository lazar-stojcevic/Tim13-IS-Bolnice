using System;
using System.Collections.Generic;

namespace IS_Bolnice.Model
{
    public class Obavestenje : Entitet
    {
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public DateTime VremeKreiranja { get; set; }
        public List<Uloge> Uloge { get; set; }
        public List<Pacijent> OdredjeniPacijenti { get; set; }

        public Obavestenje() : base(Guid.NewGuid().ToString())
        {
            Uloge = new List<Uloge>();
            OdredjeniPacijenti = new List<Pacijent>();
        }
    }
}