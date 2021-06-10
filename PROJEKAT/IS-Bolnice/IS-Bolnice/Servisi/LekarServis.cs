using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    class LekarServis: ILogInServis
    {
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();
        public List<Lekar> GetSviLekariSpecijalisti()
        {
            return lekarRepo.GetSviLekariSpecijalisti();
        }

        public List<Lekar> GetSviLekariOpstePrakse()
        {
            return lekarRepo.GetSviLekariOpstePrakse();
        }

        public List<Lekar> GetSviLekari()
        {
            return lekarRepo.GetSve();
        }

        public Lekar GetLekar(string jmbgLekara)
        {
            return lekarRepo.GetPoId(jmbgLekara);
        }

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
