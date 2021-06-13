using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Korisnici
{
    class SekretarServis
    {
        private ISekretarRepozitorijum repo = new Injector().GetSekretarRepozitorijum();
        
        public Sekretar GetByJmbg(string jmbg)
        {
            return repo.GetPoId(jmbg);
        }
    }
}
