using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class SadrzajSobeServis
    {
        private ISadrzajSobeRepozitorijum sadrzajSobeRepo = new SadrzajSobeFajlRepozitorijum();
        private IBolnicaRepozitorijum bolnicaRepozitorijum = new BolnicaFajlRepozitorijum();
        public List<SadrzajSobe> GetSadrzajSobe(string idSobe)
        {
            return sadrzajSobeRepo.GetSadrzajSobe(idSobe);
        }

        public List<SadrzajSobe> GetDinamickiSadrzajSobe(string idSobe)
        {
            return sadrzajSobeRepo.GetDinamickiSadrzajSobe(idSobe);
        }

        public bool PostojiOpremaUBolnici(string idPredmeta)
        {
            List<SadrzajSobe> sadrzajSoba = sadrzajSobeRepo.DobaviSve();
            foreach (SadrzajSobe sadrzaj in sadrzajSoba)
            {
                if (sadrzaj.Predmet.Id.Equals(idPredmeta))
                {
                    return true;
                }
            }
            return false;
        }

        public int BrojKrevetaUSobi(string idSobe)
        {
            List<SadrzajSobe> operemaUSObi = sadrzajSobeRepo.GetSadrzajSobe(idSobe);
            foreach (SadrzajSobe predmet in operemaUSObi)
            {
                if (predmet.Predmet.Id.Equals("1111"))
                {
                    return predmet.Kolicina;
                }
            }

            return 0;
        }


        private List<SadrzajSobe> SadrzajiUPrenosu()
        {
            List<SadrzajSobe> sviSadrzaji = sadrzajSobeRepo.DobaviSve();
            List<SadrzajSobe> sadrzajiUPrenosu = new List<SadrzajSobe>();
            foreach (SadrzajSobe sadrzaj in sviSadrzaji)
            {
                if (sadrzaj.NovaSoba != null)
                {
                    sadrzajiUPrenosu.Add(sadrzaj);
                }
            }
            return sadrzajiUPrenosu;
        }

        public void IzvrsiTransport()
        {
            List<SadrzajSobe> sadrzaji = SadrzajiUPrenosu();
            foreach (SadrzajSobe sadrzaj in sadrzaji)
            {
                if (sadrzaj.DatumPremestanja < DateTime.UtcNow)
                {
                    if (!DodajOpremuNaStanje(sadrzaj))
                    {
                        SadrzajSobe noviSadrzaj = new SadrzajSobe(sadrzaj.NovaSoba.Id, sadrzaj.Predmet.Id, sadrzaj.Kolicina);
                        sadrzajSobeRepo.Sacuvaj(noviSadrzaj);
                    }
                    sadrzajSobeRepo.Obrisi(sadrzaj.Id);

                }
            }
        }

        private bool DodajOpremuNaStanje(SadrzajSobe sadrzajUPrenosu)
        {
            bool opremaPostojiUProstoriji = false;
            List<SadrzajSobe> novaSoba = sadrzajSobeRepo.GetSadrzajSobe(sadrzajUPrenosu.NovaSoba.Id);
            foreach (SadrzajSobe sadrzaj in novaSoba)
            {
                if (sadrzaj.Predmet.Id.Equals(sadrzajUPrenosu.Predmet.Id))
                {
                    sadrzaj.Kolicina += sadrzajUPrenosu.Kolicina;
                    sadrzajSobeRepo.Izmeni(sadrzaj);
                    opremaPostojiUProstoriji = true;
                    break;
                }

            }
            return opremaPostojiUProstoriji;
        }

        public void DodajUMagacin(Predmet p, int kolicina)
        {
            List<SadrzajSobe> sadrzajSobe = sadrzajSobeRepo.GetSadrzajSobe(bolnicaRepozitorijum.GetMagacin().Id);
            if (OpremaPostojiUMagaciju(sadrzajSobe, p))
            {
                SadrzajSobe s = IzmenaSadrzaja(p, kolicina);
                sadrzajSobeRepo.Izmeni(s);
            }
            else
            {
                SadrzajSobe noviSadrzaj = new SadrzajSobe(bolnicaRepozitorijum.GetMagacin().Id, p.Id, kolicina);
                sadrzajSobeRepo.Sacuvaj(noviSadrzaj);
            }
        }

        private bool OpremaPostojiUMagaciju(List<SadrzajSobe> sadrzajSobe, Predmet predmet)
        {
            bool postoji = false;
            foreach (SadrzajSobe sadrzaj in sadrzajSobe)
            {
                if (sadrzaj.Predmet.Id.Equals(predmet.Id))
                {
                    postoji = true;
                }
            }

            return postoji;
        }

        private SadrzajSobe IzmenaSadrzaja(Predmet predmet, int kolicina)
        {
            List<SadrzajSobe> sviSadrzaji = sadrzajSobeRepo.GetSadrzajSobe(bolnicaRepozitorijum.GetMagacin().Id);
            foreach (SadrzajSobe sadrzaj in sviSadrzaji)
            {
                if (sadrzaj.Predmet.Id.Equals(predmet.Id))
                {
                    sadrzaj.Kolicina = sadrzaj.Kolicina + kolicina;
                    return sadrzaj;
                }
            }
            return new SadrzajSobe();
        }

        SadrzajSobe sadrzajZaPrenos;
        Soba sobaUKojuSePrenosi;

        public void PrebaciOpremu(SadrzajSobe stariSadrzaj, Soba novaSoba)
        {
            sadrzajZaPrenos = stariSadrzaj;
            sobaUKojuSePrenosi = novaSoba;
            DodajOdabranuKolicinuOpreme();
            OduzmiOdabranuKolicinuOpreme();

        }

        private void DodajOdabranuKolicinuOpreme()
        {
            List<SadrzajSobe> sadrzajSobe = sadrzajSobeRepo.GetSadrzajSobe(sobaUKojuSePrenosi.Id);
            foreach (SadrzajSobe sadrzaj in sadrzajSobe)
            {
                if (sadrzaj.Predmet.Id.Equals(sadrzajZaPrenos.Predmet.Id))
                {
                    sadrzaj.Kolicina = sadrzaj.Kolicina + sadrzajZaPrenos.Kolicina;
                    sadrzajSobeRepo.Izmeni(sadrzaj);
                    return;
                }
            }
            SadrzajSobe noviSadrzaj = new SadrzajSobe(sobaUKojuSePrenosi.Id, sadrzajZaPrenos.Predmet.Id, sadrzajZaPrenos.Kolicina);
            sadrzajSobeRepo.Sacuvaj(noviSadrzaj);
        }

        private void OduzmiOdabranuKolicinuOpreme()
        {
            List<SadrzajSobe> sadrzajSobe = sadrzajSobeRepo.GetSadrzajSobe(sadrzajZaPrenos.Soba.Id);
            foreach (SadrzajSobe sadrzaj in sadrzajSobe)
            {
                if (sadrzaj.Predmet.Id.Equals(sadrzajZaPrenos.Predmet.Id))
                {
                    if (sadrzaj.Kolicina >= sadrzajZaPrenos.Kolicina)
                    {
                        sadrzaj.Kolicina = sadrzaj.Kolicina - sadrzajZaPrenos.Kolicina;
                        sadrzajSobeRepo.Izmeni(sadrzaj);
                    }
                    else
                    {
                        MessageBox.Show("Odabrana kolicina nije validna!");
                    }
                }
            }
        }

        public void PrebaciOpremuUStanjeCekanja(SadrzajSobe sadrzajZaPrenosStaticka)
        {
            sadrzajZaPrenos = sadrzajZaPrenosStaticka;
            OduzmiOdabranuKolicinuOpreme();
            sadrzajSobeRepo.Sacuvaj(sadrzajZaPrenosStaticka);
        }

        public void ObrisiOpremuIzSobe(string idSobe)
        {
            List<SadrzajSobe> sviSadrzaji = sadrzajSobeRepo.GetSadrzajSobe(idSobe);
            foreach (SadrzajSobe sadrzaj in sviSadrzaji)
            {
                sadrzajSobeRepo.Obrisi(sadrzaj.Id);
            }
        }
    }
}
