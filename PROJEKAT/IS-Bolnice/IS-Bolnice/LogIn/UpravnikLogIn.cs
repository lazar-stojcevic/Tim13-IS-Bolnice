using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.DTOs;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.LogIn
{
    class UpravnikLogIn:ILogInServis
    {
        IUpravnikRepozitorijum upravnikRepo = new Injector().GetUpravnikRepozitorijum();

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
    }
}
