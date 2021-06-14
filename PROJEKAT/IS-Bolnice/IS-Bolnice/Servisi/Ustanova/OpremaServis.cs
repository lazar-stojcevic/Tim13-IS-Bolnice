using System.Collections.Generic;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi.Ustanova
{
    class OpremaServis
    {
        IOpremaRepozitorijum opremaRepo = new Injector().GetOpremaRepozitorijum();
        internal void KreirajNoviPredmet(Predmet noviPredmet)
        {
            opremaRepo.Sacuvaj(noviPredmet);
        }
        public Predmet GetPoId(string idOpreme)
        {

            return opremaRepo.GetPoId(idOpreme);
        }

        public void IzmeniPredmet(Predmet izmenjenPredmet)
        {
            opremaRepo.Izmeni(izmenjenPredmet);
        }

        public bool ObrisiPredmet(string idPredmeta)
        {
            SadrzajSobeServis sadrzajSobeServis = new SadrzajSobeServis();
            Predmet predmetZaBrisanje = opremaRepo.GetPoId(idPredmeta);
            if (!sadrzajSobeServis.PostojiOpremaUBolnici(idPredmeta))
            {
                predmetZaBrisanje.Obrisano = true;
                opremaRepo.Izmeni(predmetZaBrisanje);
                return true;
            }

            return false;
        }

        public List<Predmet> GetSvaOprema()
        {
            return opremaRepo.GetSve();
        }
    }
}
