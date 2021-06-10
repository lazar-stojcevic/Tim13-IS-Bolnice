using IS_Bolnice.Baze.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class UpravnikServis: ILogInServis
    {
        IUpravnikRepozitorijum upravnikRepo = new Injector().GetUpravnikRepozitorijum();

        public void Izmeni(Upravnik upravnik) {

            upravnikRepo.Izmeni(upravnik);
        }

        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            foreach (Upravnik u in upravnikRepo.GetSve())
            {
                if (u.KorisnickoIme.Equals(korisnickoIme))
                {
                    if (u.Sifra.Equals(sifra))
                    {
                        LogInDTO retVal = new LogInDTO();
                        retVal.Jmbg = u.Jmbg;
                        retVal.TipKorisnika = "U";
                        return retVal;
                    }
                }
            }

            return null;
        }

        public Upravnik GetByJmbg(string jmbg)
        {
            return upravnikRepo.GetPoId(jmbg);
        }
    }
}
