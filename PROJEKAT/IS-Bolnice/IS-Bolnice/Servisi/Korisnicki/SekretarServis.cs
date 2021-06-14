using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;

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
