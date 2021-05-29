using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class ObavestenjeKontroler
    {
        private ObavestenjeServis obavestenjeServis = new ObavestenjeServis();

        public void KreirajObavestenje(Obavestenje novoObavestenje)
        {
            obavestenjeServis.KreirajObavestenje(novoObavestenje);
        }

        public void IzmeniObavestenje(Obavestenje izmenjeno)
        {
            obavestenjeServis.IzmeniObavestenje(izmenjeno);
        }

        public List<Obavestenje> GetSvaSortiranaObavestenja()
        {
            return obavestenjeServis.GetSvaSortiranaObavestenja();
        }

        public void ObrisiObavestenje(string idObavestenja)
        {
            obavestenjeServis.ObrisiObavestenje(idObavestenja);
        }
    }
}
