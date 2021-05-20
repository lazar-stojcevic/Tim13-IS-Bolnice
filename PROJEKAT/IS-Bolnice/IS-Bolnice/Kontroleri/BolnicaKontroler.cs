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
    }
}
