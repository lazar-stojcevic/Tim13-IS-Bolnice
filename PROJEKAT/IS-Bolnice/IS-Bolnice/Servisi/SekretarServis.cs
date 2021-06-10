using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class SekretarServis:ILogInServis
    {
        private ISekretarRepozitorijum repo = new Injector().GetSekretarRepozitorijum();
        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            foreach (Sekretar s in repo.GetSve())
            {
                if (s.KorisnickoIme.Equals(korisnickoIme))
                {
                    if (s.Sifra.Equals(sifra))
                    {
                        LogInDTO retVal = new LogInDTO();
                        retVal.Jmbg = s.Jmbg;
                        retVal.TipKorisnika = "S";
                        return retVal;
                    }
                }
            }

            return null;
        }
    }
}
