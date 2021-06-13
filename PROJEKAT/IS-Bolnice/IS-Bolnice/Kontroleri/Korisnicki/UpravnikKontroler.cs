using IS_Bolnice.Servisi.Korisnici;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class UpravnikKontroler
    {
        UpravnikServis upravnikServis = new UpravnikServis();

        public void Izmeni(Upravnik upravnik) {
            upravnikServis.Izmeni(upravnik);
        }

        public Upravnik GetByJmbg(string jmbg)
        {
            return upravnikServis.GetByJmbg(jmbg);
        }
    }
}
