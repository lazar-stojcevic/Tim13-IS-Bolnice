using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class BelezkeServis
    {
        private IBeleskaRepozitorijum beleskaRepo = new BeleskaFajlRepozitorijum();

        public void IzmeniBelezku(Beleska izmenjenaBeleska)
        {
            beleskaRepo.Izmeni(izmenjenaBeleska);
        }

        public void SacuvajBelezku(Beleska beleska)
        {
            beleskaRepo.Sacuvaj(beleska);
        }

        public List<Beleska> GetSveTrenutneBelezkePacijenta(string jmbgPacijenta)
        {
            List<Beleska> pacijentoveBelezke = new List<Beleska>();

            foreach (Beleska belezka in beleskaRepo.GetSve())
            {
                if (TrajeLiBelezka(belezka) && belezka.Pacijent.Jmbg == jmbgPacijenta)
                {
                    pacijentoveBelezke.Add(belezka);
                }
            }

            return pacijentoveBelezke;
        }

        public void ObrisiBelezku(string idBeleske)
        {
            beleskaRepo.Obrisi(idBeleske);
        }

        private bool TrajeLiBelezka(Beleska beleska)
        {
            DateTime vremeKrajaTrajanjaBelezke = beleska.VremePocetkaVazenja.AddDays(beleska.PeriodVazenja);

            if (vremeKrajaTrajanjaBelezke > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}
