using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Korisnici
{
    class KorisnikServis
    {
        private IUpravnikRepozitorijum upravnikRepo = new Injector().GetUpravnikRepozitorijum();
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();
        private ISekretarRepozitorijum sekretarRepo = new Injector().GetSekretarRepozitorijum();
        private IPacijentRepozitorijum pacijentRepo = new Injector().GetPacijentRepozitorijum();

        public bool JedinstvenNoviJmbgKorisnika(string noviJmbg, string stariJmbg)
        {
            if (noviJmbg.Equals(stariJmbg))
            {
                return true;
            }

            return JedinstvenJmbgKorisnika(noviJmbg);
        }

        public bool JedinstvenJmbgKorisnika(string jmbg)
        {
            if (!JedinstvenJmbgUpravnika(jmbg)) return false;

            if (!JedinstvenJmbgLekara(jmbg)) return false;

            if (!JedinstvenJmbgSekretara(jmbg)) return false;

            if (!JedinstvenJmbgPacijenta(jmbg)) return false;

            return true;
        }

        public bool JedinstvenoNovoKorisnickoImeKroisnika(string novoKorisnickoIme, string staroKorisnickoIme)
        {
            if (novoKorisnickoIme.Equals(staroKorisnickoIme))
            {
                return true;
            }

            return JedinstvenoKorisnickoImeKorisnika(novoKorisnickoIme);
        }

        public bool JedinstvenoKorisnickoImeKorisnika(string korisnickoIme)
        {
            if (!JedinsvenoKorisnickoImeUpravnika(korisnickoIme)) return false;

            if (!JedinstvenoKorisnickoImeLekara(korisnickoIme)) return false;

            if (!JedinstvenoKorisnickoImeSekretara(korisnickoIme)) return false;

            if (!JedinstvenoKorisnickoImePacijenta(korisnickoIme)) return false;

            return true;
        }

        private bool JedinstvenoKorisnickoImePacijenta(string korisnickoIme)
        {
            foreach (var pacijent in pacijentRepo.GetSve())
            {
                if (pacijent.KorisnickoIme.Equals(korisnickoIme))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenoKorisnickoImeSekretara(string korisnickoIme)
        {
            foreach (var sekretar in sekretarRepo.GetSve())
            {
                if (sekretar.KorisnickoIme.Equals(korisnickoIme))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenoKorisnickoImeLekara(string korisnickoIme)
        {
            foreach (var lekar in lekarRepo.GetSve())
            {
                if (lekar.KorisnickoIme.Equals(korisnickoIme))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinsvenoKorisnickoImeUpravnika(string korisnickoIme)
        {
            foreach (var upravnik in upravnikRepo.GetSve())
            {
                if (upravnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenJmbgPacijenta(string jmbg)
        {
            foreach (var pacijent in pacijentRepo.GetSve())
            {
                if (pacijent.Jmbg.Equals(jmbg))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenJmbgSekretara(string jmbg)
        {
            foreach (var sekretar in sekretarRepo.GetSve())
            {
                if (sekretar.Jmbg.Equals(jmbg))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenJmbgLekara(string jmbg)
        {
            foreach (var lekar in lekarRepo.GetSve())
            {
                if (lekar.Jmbg.Equals(jmbg))
                {
                    return false;
                }
            }

            return true;
        }

        private bool JedinstvenJmbgUpravnika(string jmbg)
        {
            foreach (var upravnik in upravnikRepo.GetSve())
            {
                if (upravnik.Jmbg.Equals(jmbg))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
