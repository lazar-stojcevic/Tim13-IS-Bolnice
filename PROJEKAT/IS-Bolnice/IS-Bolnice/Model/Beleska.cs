using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    public class Beleska
    {
        public string Naziv { get; set; }
        public Pacijent Pacijent { get; set; }
        public string Komentar { get; set; }
        public DateTime VremePocetkaVazenja { get; set; }
        public int PeriodVazenja { get; set; }

        public Beleska()
        {
            VremePocetkaVazenja = DateTime.Now;
        }

        public Beleska(Pacijent pacijent, string komentar, DateTime vremePocetkaVazenja, int periodVazenja, string naziv)
        {
            Naziv = naziv;
            Pacijent = pacijent;
            Komentar = komentar;
            VremePocetkaVazenja = vremePocetkaVazenja;
            PeriodVazenja = periodVazenja;
        }

        public bool Iste(Beleska beleska)
        {
            return this.Pacijent.Jmbg == beleska.Pacijent.Jmbg && this.VremePocetkaVazenja == beleska.VremePocetkaVazenja;
        }
    }
}
