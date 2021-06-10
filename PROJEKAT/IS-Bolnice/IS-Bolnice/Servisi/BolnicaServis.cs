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
        private IBolnicaRepozitorijum bolnicaRepo = new Injector().GetBolnicaRepozitorijum();

        public List<Soba> GetSveSobe()
        {
            List<Soba> oneKojeNisuLogickiObrisane = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSobe())
            {
                if (!soba.Obrisano)
                {
                    oneKojeNisuLogickiObrisane.Add(soba);
                }
            }

            return oneKojeNisuLogickiObrisane;
        }

        public List<Soba> GetSveOperacioneSale()
        {
            List<Soba> oneKojeNisuLogickiObrisane = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSveOperacioneSale())
            {
                if (!soba.Obrisano)
                {
                    oneKojeNisuLogickiObrisane.Add(soba);
                }
            }
            return oneKojeNisuLogickiObrisane;
        }

        public List<Soba> GetSveSobeZaPregled()
        {
            List<Soba> oneKojeNisuLogickiObrisane = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSveSobeZaPregled())
            {
                if (!soba.Obrisano)
                {
                    oneKojeNisuLogickiObrisane.Add(soba);
                }
            }
            return oneKojeNisuLogickiObrisane;
        }

        public List<Soba> GetSveSobeZaHospitalizaciju()
        {
            List<Soba> oneKojeNisuLogickiObrisane = new List<Soba>();
            foreach (Soba soba in bolnicaRepo.GetSveSobeZaHospitalizaciju())
            {
                if (!soba.Obrisano)
                {
                    oneKojeNisuLogickiObrisane.Add(soba);
                }
            }
            return oneKojeNisuLogickiObrisane;
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


        public Soba GetSobaPoId(string idSobe)
        {
            return bolnicaRepo.GetSobaById(idSobe);
        }

        public void IzmeniSobu(Soba izmenjenaSoba)
        {
            Bolnica ovaBolnica = bolnicaRepo.GetBolnica();
            List<Soba> listaSoba = ovaBolnica.Sobe;
            foreach (Soba iterSoba in listaSoba)
            {
                if (iterSoba.Id.Equals(izmenjenaSoba.Id))
                {
                    ovaBolnica.RemoveSoba(iterSoba);
                    ovaBolnica.AddSoba(izmenjenaSoba);
                    break;
                }
            }
            bolnicaRepo.Izmeni(ovaBolnica);
        }

        public Soba GetMagacin()
        {
            return bolnicaRepo.GetMagacin();
        }

        public void ObrisiSobu(string id)
        {
            Bolnica bolnica = bolnicaRepo.GetBolnica();
            foreach (Soba sobaIter in bolnica.Sobe)
            {
                if (sobaIter.Id.Equals(id))
                {
                    sobaIter.Obrisano = true;
                }
            }
            bolnicaRepo.Izmeni(bolnica);

        }
    }
}
