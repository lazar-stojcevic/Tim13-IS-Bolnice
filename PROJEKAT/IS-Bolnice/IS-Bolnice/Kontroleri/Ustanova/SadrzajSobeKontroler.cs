using System.Collections.Generic;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Kontroleri.Ustanova
{
    class SadrzajSobeKontroler
    {
        private SadrzajSobeServis sadrzajSobeServis = new SadrzajSobeServis();

        public List<SadrzajSobe> GetSadrzajSobe(string idSobe)
        {
            return sadrzajSobeServis.GetSadrzajSobe(idSobe);
        }

        public List<SadrzajSobe> GetDnamickiSadrzajSobe(string idSobe)
        {
            return sadrzajSobeServis.GetDinamickiSadrzajSobe(idSobe);
        }

        public void IzvrsiTransport() {
            sadrzajSobeServis.IzvrsiTransport();
        }

        public void DodajUMagacin(Predmet p, int kolicina)
        {
            sadrzajSobeServis.DodajUMagacin(p, kolicina);
        }

        public bool PrebaciOpremu(SadrzajSobe stariSadrzaj, Soba novaSoba)
        {
            return sadrzajSobeServis.PrebaciOpremu(stariSadrzaj, novaSoba);
        }

        public void PrebaciOpremuUStanjeCekanja(SadrzajSobe sadrzajZaPrenos)
        {
            sadrzajSobeServis.PrebaciOpremuUStanjeCekanja(sadrzajZaPrenos);
        }
    }
}
