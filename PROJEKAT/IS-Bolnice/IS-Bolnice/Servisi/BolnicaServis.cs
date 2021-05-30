using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class BolnicaServis
    {
        private IBolnicaRepozitorijum bolnicaRepo = new BolnicaFajlRepozitorijum();

        public List<Soba> GetSveSobe()
        {
            return bolnicaRepo.GetSobe();
        }

        public List<Soba> GetSveOperacioneSale()
        {
            List<Soba> sveOperacioneSale = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSobe())
            {
                if (soba.Tip.Equals(RoomType.operacionaSala))
                {
                    sveOperacioneSale.Add(soba);
                }
            }
            return sveOperacioneSale;
        }

        internal void KreirajSobuUBolnici(Soba novaSoba)
        {
            Bolnica izmenjenaBolnica = bolnicaRepo.GetBolnica();
            int flag = 0;
            foreach (Soba s in izmenjenaBolnica.Soba)
            {
                if (s.Id.Equals(novaSoba.Id))
                {
                    if (s.Obrisano == true)
                    {
                        s.Obrisano = false;
                        flag = 1;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Soba sa izabranim ID vec postoji!");
                        flag = 2;
                    }

                }

            }
            if (flag == 0)
            {
                izmenjenaBolnica.AddSoba(novaSoba);
                bolnicaRepo.Izmeni(izmenjenaBolnica);
            }
            else if (flag == 1)
            {
                bolnicaRepo.Izmeni(izmenjenaBolnica);
            }
        }

        public List<Soba> GetSveSobeZaPregled()
        {
            List<Soba> sveSobeZaPregled = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSobe())
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
            foreach (Soba soba in bolnicaRepo.GetSobe())
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
