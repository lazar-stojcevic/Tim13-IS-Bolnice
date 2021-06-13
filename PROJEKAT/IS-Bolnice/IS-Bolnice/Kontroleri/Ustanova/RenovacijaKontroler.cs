using System.Collections.Generic;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Kontroleri.Ustanova
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
