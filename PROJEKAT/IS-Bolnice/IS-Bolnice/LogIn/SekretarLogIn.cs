﻿using System;
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
    class SekretarLogIn:ILogInServis
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
