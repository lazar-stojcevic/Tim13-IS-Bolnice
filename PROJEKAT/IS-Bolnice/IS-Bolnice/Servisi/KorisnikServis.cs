using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class KorisnikServis
    {
        private BazaUpravnika bazaUpravnika = new BazaUpravnika();
        private BazaLekara bazaLekara = new BazaLekara();
        private BazaSekretara bazaSekretara = new BazaSekretara();
        private IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();

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
            foreach (var pacijent in pacijentRepo.DobaviSve())
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
            foreach (var sekretar in bazaSekretara.SviSekretari())
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
            foreach (var lekar in bazaLekara.DobaviSve())
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
            foreach (var upravnik in bazaUpravnika.SviUpravnici())
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
            foreach (var pacijent in pacijentRepo.DobaviSve())
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
            foreach (var sekretar in bazaSekretara.SviSekretari())
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
            foreach (var lekar in bazaLekara.DobaviSve())
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
            foreach (var upravnik in bazaUpravnika.SviUpravnici())
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
