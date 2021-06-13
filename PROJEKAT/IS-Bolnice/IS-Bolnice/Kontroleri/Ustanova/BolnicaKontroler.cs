using System.Collections.Generic;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Kontroleri.Ustanova
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
