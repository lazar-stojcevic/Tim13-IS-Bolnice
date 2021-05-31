using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    public class Beleska : Entitet
    {
        public string Naziv { get; set; }
        public Pacijent Pacijent { get; set; }
        public string Komentar { get; set; }
        public DateTime VremePocetkaVazenja { get; set; }
        public int PeriodVazenja { get; set; }

        public Beleska() : base(Guid.NewGuid().ToString())
        {
            VremePocetkaVazenja = DateTime.Now;
        }

        public Beleska(Pacijent pacijent, string komentar, DateTime vremePocetkaVazenja, int periodVazenja, string naziv) : base(Guid.NewGuid().ToString())
        {
            Naziv = naziv;
            Pacijent = pacijent;
            Komentar = komentar;
            VremePocetkaVazenja = vremePocetkaVazenja;
            PeriodVazenja = periodVazenja;
        }

        public Beleska(string id, Pacijent pacijent, string komentar, DateTime vremePocetkaVazenja, int periodVazenja, string naziv) : base(id)
        {
            Naziv = naziv;
            Pacijent = pacijent;
            Komentar = komentar;
            VremePocetkaVazenja = vremePocetkaVazenja;
            PeriodVazenja = periodVazenja;
        }

        public Beleska(Beleska beleska) : base(beleska.Id)
        {
            Naziv = beleska.Naziv;
            Pacijent = beleska.Pacijent;
            Komentar = beleska.Komentar;
            VremePocetkaVazenja = beleska.VremePocetkaVazenja;
            PeriodVazenja = beleska.PeriodVazenja;
        }

        public bool Iste(Beleska beleska)
        {
            return this.Pacijent.Jmbg == beleska.Pacijent.Jmbg && this.VremePocetkaVazenja == beleska.VremePocetkaVazenja;
        }
    }
}
