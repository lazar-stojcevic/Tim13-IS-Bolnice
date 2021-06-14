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
    class PacijentLogIn:ILogInServis
    {
        private IPacijentRepozitorijum pacijentRepo = new Injector().GetPacijentRepozitorijum();
        public LogInDTO GetKorisnika(string korisnickoIme, string sifra)
        {
            foreach (Pacijent p in pacijentRepo.GetSve())
            {
                if (p.KorisnickoIme.Equals(korisnickoIme))
                {
                    if (p.Sifra.Equals(sifra))
                    {
                        LogInDTO retVal = new LogInDTO();
                        retVal.Jmbg = p.Jmbg;
                        retVal.TipKorisnika = "P";
                        return retVal;
                    }
                }
            }

            return null;
        }
    }
}
