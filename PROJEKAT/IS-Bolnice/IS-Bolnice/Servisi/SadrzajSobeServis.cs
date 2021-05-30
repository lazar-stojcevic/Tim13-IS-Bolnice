using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class SadrzajSobeServis
    {
        private ISadrzajSobeRepozitorijum sadrzajSobeRepo = new SadrzajSobeFajlRepozitorijum();

        public List<SadrzajSobe> GetSadrzajSobe(string idSobe)
        {
            return sadrzajSobeRepo.GetSadrzajSobe(idSobe);
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
    }
}
