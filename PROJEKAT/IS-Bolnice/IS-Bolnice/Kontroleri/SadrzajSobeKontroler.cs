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

        public void IzvrsiTransport() {
            sadrzajSobeServis.IzvrsiTransport();
        }
    }
}
