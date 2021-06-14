using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;
using IS_Bolnice.LogIn;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.LogIn
{
    class LekarLogIn:ILogInServis
    {
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();

        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            foreach (Lekar l in lekarRepo.GetSve())
            {
                if (l.KorisnickoIme.Equals(korisnickoIme))
                {
                    if (l.Sifra.Equals(sifra))
                    {
                        LogInDTO retVal = new LogInDTO();
                        retVal.Jmbg = l.Jmbg;
                        retVal.TipKorisnika = "L";
                        return retVal;
                    }
                }
            }

            return null;
        }
    }
}
