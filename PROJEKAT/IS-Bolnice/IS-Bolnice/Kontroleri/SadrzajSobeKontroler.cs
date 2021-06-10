using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
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
