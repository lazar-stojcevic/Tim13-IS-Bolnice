using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class ObavestenjeServis
    {
        private IObavestenjaRepozitorijum obavestenjaRepo = new ObavestenjeFajlRepozitorijum();

        public void KreirajObavestenje(Obavestenje novoObavestenje)
        {
            obavestenjaRepo.Sacuvaj(novoObavestenje);
        }

        public void IzmeniObavestenje(Obavestenje izmenjeno)
        {
            obavestenjaRepo.Izmeni(izmenjeno);
        }

        public List<Obavestenje> GetSvaSortiranaObavestenja()
        {
            List<Obavestenje> svaObavestenja = obavestenjaRepo.DobaviSve();
            svaObavestenja.Sort((o1, o2) => DateTime.Compare(o1.VremeKreiranja,o2.VremeKreiranja));
            svaObavestenja.Reverse();
            return svaObavestenja;
        }

        public void ObrisiObavestenje(string idObavestenja)
        {
            obavestenjaRepo.Obrisi(idObavestenja);
        }
    }
}
