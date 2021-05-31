using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class OpremaServis
    {
        IOpremaRepozitorijum opremaRepo = new OpremaFajlRepozitorijum();
        internal void KreirajNoviPredmet(Predmet noviPredmet)
        {
            opremaRepo.Sacuvaj(noviPredmet);
        }
        public Predmet DodaviPoID(string idOpreme)
        {

            return opremaRepo.DobaviPoId(idOpreme);
        }

        public void IzmeniPredmet(Predmet izmenjenPredmet)
        {
            opremaRepo.Izmeni(izmenjenPredmet);
        }

        public void ObrisiPredmet(string idPredmeta)
        {
            SadrzajSobeServis sadrzajSobeServis = new SadrzajSobeServis();
            Predmet predmetZaBrisanje = opremaRepo.DobaviPoId(idPredmeta);
            if (!sadrzajSobeServis.PostojiOpremaUBolnici(idPredmeta))
            {
                predmetZaBrisanje.Obrisano = true;
                opremaRepo.Izmeni(predmetZaBrisanje);

            }
            else
            {
                MessageBox.Show("Oprema postoji na stanju, ne može biti obrisana!");
            }
        }

        public List<Predmet> DobaviSvuOpremu()
        {
            return opremaRepo.DobaviSve();
        }
    }
}
