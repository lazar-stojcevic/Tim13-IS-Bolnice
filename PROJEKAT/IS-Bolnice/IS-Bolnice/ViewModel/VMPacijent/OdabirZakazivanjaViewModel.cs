using IS_Bolnice.Prozori.Prozori_za_pacijenta;

namespace IS_Bolnice.ViewModel.VMPacijent
{
    class OdabirZakazivanjaViewModel : ViewModel
    {
        private OdabirZakazivanja odabirZakazivanjaProzor;
        private string jmbgPacijenta;

        public OdabirZakazivanjaViewModel(OdabirZakazivanja odabirZakazivanja, string jmbgPacijenta)
        {
            this.odabirZakazivanjaProzor = odabirZakazivanja;
            this.jmbgPacijenta = jmbgPacijenta;

            Odustani = new RelayCommand(IzvrsiOdustaniKomandu);
            ZakazivanjeKodLekara = new RelayCommand(IzvrsiZakazivanjeKodLekara);
            ZakazivanjeUTerminu = new RelayCommand(IzvrsiZakazivanjeUTerminu);

        }

        public RelayCommand Odustani
        {
            get;
            set;
        }

        public RelayCommand ZakazivanjeKodLekara
        {
            get;
            set;
        }

        public RelayCommand ZakazivanjeUTerminu
        {
            get;
            set;
        }

        public void IzvrsiZakazivanjeKodLekara(object obj)
        {
            ZakazivanjeKodOdredjenogLekara zkol = new ZakazivanjeKodOdredjenogLekara(jmbgPacijenta);
            zkol.ShowDialog();
        }

        public void IzvrsiZakazivanjeUTerminu(object obj)
        {
            ZakazivanjeUOdredjenomTerminu zuot = new ZakazivanjeUOdredjenomTerminu(jmbgPacijenta);
            zuot.ShowDialog();
        }

        public void IzvrsiOdustaniKomandu(object obj)
        {
            odabirZakazivanjaProzor.Close();
        }
    }
}
