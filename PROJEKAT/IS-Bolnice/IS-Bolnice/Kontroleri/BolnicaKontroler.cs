using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class BolnicaKontroler
    {
        private BolnicaServis bolnicaServis = new BolnicaServis();

        public void KreirajSobuUBolnici(Soba novaSoba) {
            bolnicaServis.KreirajSobuUBolnici(novaSoba);
        }

        public List<Soba> GetSveSobe()
        {
            return bolnicaServis.GetSveSobe();
        }

        public List<Soba> GetSveOperacioneSale()
        {
            return bolnicaServis.GetSveOperacioneSale();
        }

        public List<Soba> GetSveSveSobeZaPregled()
        {
            return bolnicaServis.GetSveSobeZaPregled();
        }

        public List<Soba> GetSveSobeZaHospitalizaciju()
        {
            return bolnicaServis.GetSveSobeZaHospitalizaciju();
        }

        public Soba GetSobaPoId(string idSobe)
        {
            return bolnicaServis.GetSobaPoId(idSobe);
        }

        public void IzmeniSobu(Soba izmenjenaSoba)
        {
            bolnicaServis.IzmeniSobu(izmenjenaSoba);
        }

        public Soba GetMagacin()
        {
            return bolnicaServis.GetMagacin();
        }

        public void ObrisiSobu(string id)
        {
            bolnicaServis.ObrisiSobu(id);
        }
    }
}
