using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class ObavestenjeServis
    {
        private BazaObavestenja bazaObavestenja = new BazaObavestenja();

        public void KreirajObavestenje(Obavestenje novoObavestenje)
        {
            bazaObavestenja.KreirajObavestenje(novoObavestenje);
        }

        public void IzmeniObavestenje(Obavestenje izmenjeno)
        {
            bazaObavestenja.IzmeniObavestenje(izmenjeno);
        }

        public List<Obavestenje> GetSvaSortiranaObavestenja()
        {
            List<Obavestenje> svaObavestenja = bazaObavestenja.SvaObavestenja();
            svaObavestenja.Sort((o1, o2) => DateTime.Compare(o1.VremeKreiranja,o2.VremeKreiranja));
            svaObavestenja.Reverse();
            return svaObavestenja;
        }

        public void ObrisiObavestenje(Obavestenje obavestenje)
        {
            bazaObavestenja.ObrisiObavestenje(obavestenje);
        }
    }
}
