using IS_Bolnice.Servisi.Korisnici;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class KorisnikKontroler
    {
        private KorisnikServis korisnikServis = new KorisnikServis();

        public bool JedinstvenJmbgKorisnika(string jmbg)
        {
            return korisnikServis.JedinstvenJmbgKorisnika(jmbg);
        }

        public bool JedinstvenNoviJmbgKorisnika(string noviJmbg, string stariJmbg)
        {
            return korisnikServis.JedinstvenNoviJmbgKorisnika(noviJmbg, stariJmbg);
        }

        public bool JedinstvenoKorisnickoImeKorisnika(string korisnickoIme)
        {
            return korisnikServis.JedinstvenoKorisnickoImeKorisnika(korisnickoIme);
        }

        public bool JedinstvenoNovoKorisnickoImeKroisnika(string novoKorisnickoIme, string staroKorisnickoIme)
        {
            return korisnikServis.JedinstvenoNovoKorisnickoImeKroisnika(novoKorisnickoIme, staroKorisnickoIme);
        }
    }
}
