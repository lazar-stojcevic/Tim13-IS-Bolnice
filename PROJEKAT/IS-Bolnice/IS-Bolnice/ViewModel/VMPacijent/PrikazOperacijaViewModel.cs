using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using System.Collections.Generic;

namespace IS_Bolnice.ViewModel.VMPacijent
{
    class PrikazOperacijaViewModel
    {
        private PrikazTerminaOperacija prikazTerminaOperacijaProzor;
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();

        public PrikazOperacijaViewModel(PrikazTerminaOperacija prikazTerminaOperacija, string jmbgPacijenta)
        {
            this.prikazTerminaOperacijaProzor = prikazTerminaOperacija;

            OperacijePacijenta = operacijaKontroler.GetSveBuduceOperacijePacijenta(jmbgPacijenta);

            Izadji = new RelayCommand(IzvrsiIzadjiKomandu);
        }

        public List<Operacija> OperacijePacijenta { get; set; }
        
        public RelayCommand Izadji { get; set; }

        public void IzvrsiIzadjiKomandu(object obj)
        {
            prikazTerminaOperacijaProzor.Close();
        }
    }
}
