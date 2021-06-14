using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Servisi.Korisnici
{
    class UpravnikServis
    {
        IUpravnikRepozitorijum upravnikRepo = new Injector().GetUpravnikRepozitorijum();

        public void Izmeni(Upravnik upravnik) {

            upravnikRepo.Izmeni(upravnik);
        }


        public Upravnik GetByJmbg(string jmbg)
        {
            return upravnikRepo.GetPoId(jmbg);
        }
    }
}
