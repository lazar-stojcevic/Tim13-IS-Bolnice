using IS_Bolnice.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Kontroleri
{
    class RenovacijaKontroler
    {
        RenovacijaServis servis = new RenovacijaServis();
        public bool RenoviranjeOperacioneSale(Renovacija novaRenovacija)
        {
            return servis.RenoviranjeOperacioneSale(novaRenovacija);
        }

        public bool RenoviranjeProstorije(Renovacija novaRenovacija)
        {
            return servis.RenoviranjeProstorije(novaRenovacija);
        }

        public bool RenovirajBolnickuSobu(Renovacija novaRenovacija)
        {
            return servis.RenovirajBolnickuSobu(novaRenovacija);
        }

        public void RenovirajOpremu(int oznakaRadnjeNadOpremom, string idSobe)
        {
            servis.RenovirajOpremu(oznakaRadnjeNadOpremom, idSobe);
        }

        public bool SpajanjeSobe(List<Soba> sobeZaSpajanje, Renovacija renovacija)
        {
            return servis.SpajanjeSobe(sobeZaSpajanje, renovacija);
        }

        public void RazdvajanjeSobe(Renovacija renovacija, List<Soba> noveSobe) 
        {
            servis.RazdvajanjeSobe(renovacija, noveSobe);
        }
    }
}
