using IS_Bolnice.Servisi.Korisnici;

namespace IS_Bolnice.Kontroleri.Korisnicki
{
    class SekretarKontroler
    {
        private SekretarServis sekretarServis = new SekretarServis();

        public Sekretar GetByJmbg(string jmbg)
        {
            return sekretarServis.GetByJmbg(jmbg);
        }
    }
}
