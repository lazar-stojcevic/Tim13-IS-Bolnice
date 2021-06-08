using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.ViewModel.Lekar
{
    class GlavniMeniViewModel : ViewModel
    {
        private WPFNavigacijaKontroler navigacijaKontroler;

        private string sifra;

        public WPFNavigacijaKontroler NavigacijaKontroler
        {
            get { return navigacijaKontroler; }
            set
            {
                navigacijaKontroler = value;
                OnPropertyChanged();
            }
        }

        public string Sifra
        {
            get { return sifra; }
            set
            {
                sifra = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand pocniPregled;

        private RelayCommand idiNaRaspored;

        private RelayCommand inventarPoSalama;

        private RelayCommand uvidURecenzije;

        private RelayCommand zahtevZaGodinji;

        private RelayCommand validacijaLekova;

        private RelayCommand izvestajOPotrosnji;

        private RelayCommand uvidUOdobreneLekove;

        private RelayCommand uvidUHospitalizacije;

        private RelayCommand odjaviSe;

        private RelayCommand recenzijaAplikacije;

        private RelayCommand obavestenja;

        public RelayCommand PocniPregled
        {
            get { return pocniPregled; }
            set { pocniPregled = value; }
        }

        public RelayCommand IdiNaRaspored
        {
            get { return idiNaRaspored; }
            set { idiNaRaspored = value; }
        }

        public RelayCommand InventarPoSalama
        {
            get { return inventarPoSalama; }
            set { inventarPoSalama = value; }
        }

        public RelayCommand UvidURecenzije
        {
            get { return uvidURecenzije; }
            set { uvidURecenzije = value; }
        }

        public RelayCommand ZahtevZaGodinji
        {
            get { return zahtevZaGodinji; }
            set { zahtevZaGodinji = value; }
        }

        public RelayCommand ValidacijaLekova
        {
            get { return validacijaLekova; }
            set { validacijaLekova = value; }
        }

        public RelayCommand IzvestajOPotrosnji
        {
            get { return izvestajOPotrosnji; }
            set { izvestajOPotrosnji = value; }
        }

        public RelayCommand UvidUOdobreneLekove
        {
            get { return uvidUOdobreneLekove; }
            set { uvidUOdobreneLekove = value; }
        }

        public RelayCommand UvidUHospitalizacije
        {
            get { return uvidUHospitalizacije; }
            set { uvidUHospitalizacije = value; }
        }

        public RelayCommand OdjaviSe
        {
            get { return odjaviSe; }
            set { odjaviSe = value; }
        }

        public RelayCommand RecenzijaAplikacije
        {
            get { return recenzijaAplikacije; }
            set { recenzijaAplikacije = value; }
        }

        public RelayCommand Obavestenja
        {
            get { return obavestenja; }
            set { obavestenja = value; }
        }

        public void Execute_IdiNazad(object obj)
        {
            navigacijaKontroler.IdiNazad();
        }

        public void Execute_PocniPregled(object obj)
        {
            navigacijaKontroler.ZapocniPregled(Sifra);
        }

        public void Execute_IdiNaRaspored(object obj)
        {
            navigacijaKontroler.OtvoriRaspored(Sifra);
        }

        public void Execute_InventarPoSalama(object obg)
        {
            navigacijaKontroler.OtvoriInventare();
        }

        public void Execute_OtvoriOcene(object obj)
        {
            navigacijaKontroler.OtvoriRecenzije(Sifra);
        }

        public void Execute_ZahtevZaGodisnji(object obj)
        {
            navigacijaKontroler.OtvoriZahtevZaGodinji();
        }

        public void Execute_ValidacijeLekova(object obj)
        {
            navigacijaKontroler.OtvoriValidacijuLekova(Sifra);
        }

        public void Execute_Izvestaj(object obj)
        {
            navigacijaKontroler.OtvoriIzvestajOPotresnji();
        }

        public void Execute_OdobreniLekovi(object obj)
        {
            navigacijaKontroler.OtvoriUvidUOdobreneLekove();
        }

        public void Execute_UvidUSveHospitalizacije(object obj)
        {
            navigacijaKontroler.OtvoriSveHospitalizacije();
        }

        public void Execute_OdjaviSe(object obj)
        {
            navigacijaKontroler.OdjaviSe();
        }

        public void Execute_OstaviRecenziju(object obj)
        {
            navigacijaKontroler.OstaviRecenziju();
        }

        public void Execute_VidiObavestenja(object obj)
        {
            navigacijaKontroler.OtvoriObavestenja();
        }

        public GlavniMeniViewModel(NavigationService navigationService, string jmbgLekara)
        {
            Sifra = jmbgLekara;

            NavigacijaKontroler = new WPFNavigacijaKontroler(navigationService);

            PocniPregled = new RelayCommand(Execute_PocniPregled);

            IdiNaRaspored = new RelayCommand(Execute_IdiNaRaspored);

            InventarPoSalama = new RelayCommand(Execute_InventarPoSalama);

            UvidURecenzije = new RelayCommand(Execute_OtvoriOcene);

            ZahtevZaGodinji = new RelayCommand(Execute_ZahtevZaGodisnji);

            ValidacijaLekova = new RelayCommand(Execute_ValidacijeLekova);

            IzvestajOPotrosnji = new RelayCommand(Execute_Izvestaj);

            UvidUOdobreneLekove = new RelayCommand(Execute_OdobreniLekovi);

            UvidUHospitalizacije = new RelayCommand(Execute_UvidUSveHospitalizacije);

            OdjaviSe = new RelayCommand(Execute_OdjaviSe);

            RecenzijaAplikacije = new RelayCommand(Execute_OstaviRecenziju);

            Obavestenja = new RelayCommand(Execute_VidiObavestenja);


        }

    }
}
