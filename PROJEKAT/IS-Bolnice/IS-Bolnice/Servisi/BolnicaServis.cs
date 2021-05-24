using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class BolnicaServis
    {
        private BazaBolnica bazaBolnica = new BazaBolnica();

        public List<Soba> GetSveSobe()
        {
            return bazaBolnica.GetSobe();
        }

        public List<Soba> GetSveOperacioneSale()
        {
            List<Soba> sveOperacioneSale = new List<Soba>();
            foreach (Soba soba in bazaBolnica.GetSobe())
            {
                if (soba.Tip.Equals(RoomType.operacionaSala))
                {
                    sveOperacioneSale.Add(soba);
                }
            }
            return sveOperacioneSale;
        }

        public List<Soba> GetSveSobeZaPregled()
        {
            List<Soba> sveSobeZaPregled = new List<Soba>();
            foreach (Soba soba in bazaBolnica.GetSobe())
            {
                if (!soba.Tip.Equals(RoomType.magacin))
                {
                    sveSobeZaPregled.Add(soba);
                }
            }
            return sveSobeZaPregled;
        }

        public List<Soba> GetSveSobeZaHospitalizaciju()
        {
            List<Soba> sveSobeZAHospitalizaciju = new List<Soba>();
            foreach (Soba soba in bazaBolnica.GetSobe())
            {
                if (soba.Tip.Equals(RoomType.bolnickaSoba))
                {
                    sveSobeZAHospitalizaciju.Add(soba);
                }
            }
            return sveSobeZAHospitalizaciju;
        }

    }
}
