using IS_Bolnice.Kontroleri;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace IS_Bolnice.ViewModel.Lekar
{
    class RasporedLekaraViewModel : ViewModel
    {
        private WPFNavigacijaKontroler navigacijaKontroler;

        private string idLekara;
        private Operacija operacija;
        private Pregled pregled;

        //Kontroler
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private PregledKontroler pregledKontroler = new PregledKontroler();

        //Kolekcije
        private ObservableCollection<Operacija> operacije = new ObservableCollection<Operacija>();
        private ObservableCollection<Pregled> pregledi = new ObservableCollection<Pregled>();

        public WPFNavigacijaKontroler NavigacijaKontroler
        {
            get { return navigacijaKontroler; }
            set
            {
                navigacijaKontroler = value;
                OnPropertyChanged();
            }
        }

        public Operacija Operacija
        {
            get { return operacija; }
            set
            {
                operacija = value;
                OnPropertyChanged();
            }
        }

        public Pregled Pregled
        {
            get { return pregled; }
            set
            {
                pregled = value;
                OnPropertyChanged();
            }
        }

        public string IdLekara
        {
            get { return idLekara; }
            set
            {
                idLekara = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Operacija> Operacije
        {
            get { return operacije; }
            set
            {
                operacije = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Pregled> Pregledi
        {
            get { return pregledi; }
            set
            {
                pregledi = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand idiNazad;

        private RelayCommand zapocniPregled;

        private RelayCommand izmeniPregled;

        private RelayCommand obrisiPregled;

        private RelayCommand vidiPacijentaPregleda;

        private RelayCommand izmeniOperaciju;

        private RelayCommand obrisiOperaciju;

        private RelayCommand vidiPacijentaOperacije;

        public RelayCommand IdiNazad
        {
            get { return idiNazad; }
            set { idiNazad = value; }
        }

        public RelayCommand ZapocniPregled
        {
            get { return zapocniPregled; }
            set { zapocniPregled = value; }
        }

        public RelayCommand IzmeniPregled
        {
            get { return izmeniPregled; }
            set { izmeniPregled = value; }
        }

        public RelayCommand ObrisiPregled
        {
            get { return obrisiPregled; }
            set { obrisiPregled = value; }
        }

        public RelayCommand VidiPacijentaPregleda
        {
            get { return vidiPacijentaPregleda; }
            set { vidiPacijentaPregleda = value; }
        }

        public RelayCommand IzmeniOperaciju
        {
            get { return izmeniOperaciju; }
            set { izmeniOperaciju = value; }
        }

        public RelayCommand ObrisiOperaciju
        {
            get { return obrisiOperaciju; }
            set { obrisiOperaciju = value; }
        }

        public RelayCommand VidiPacijentaOperacije
        {
            get { return vidiPacijentaOperacije; }
            set { vidiPacijentaOperacije = value; }
        }

        public void Execute_IdiNazad(object obj)
        {
            navigacijaKontroler.IdiNazad();
        }

        public void Execute_OtvoriPregled(object obj)
        {
            navigacijaKontroler.OtvoriPregled(pregled);
        }

        public void Execute_IzmeniPregled(object obj)
        {
            navigacijaKontroler.PrikaziIzmenuPregleda(pregled);
        }

        public void Execute_IzmeniOperaciju(object obj)
        {
            navigacijaKontroler.PrikaziIzmenuOperacije(operacija);
        }

        public void Execute_ObrisiPregled(object obj)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otkažete pregled?", "Otkazivanje pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                pregledKontroler.OtkaziPregled(pregled);
                pregledi.Remove(pregled);
            }
        }

        public void Execute_ObrisiOperaciju(object obj)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otkažete operaciju?", "Otkazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                operacijaKontroler.OtkaziOperaciju(operacija);
                operacije.Remove(operacija);
            }
        }

        public void Execute_PrikaziPacijentaPregleda(object obj)
        {
            navigacijaKontroler.PrikaziPacijenta(pregled.Pacijent);
        }

        public void Execute_PrikaziPacijentaOperacije(object obj)
        {
            navigacijaKontroler.PrikaziPacijenta(operacija.Pacijent);
        }

        public RasporedLekaraViewModel(NavigationService navigationService, string jmbgLekara)
        { 
            NavigacijaKontroler = new WPFNavigacijaKontroler(navigationService);

            IdiNazad = new RelayCommand(Execute_IdiNazad);

            ZapocniPregled = new RelayCommand(Execute_OtvoriPregled);

            IzmeniPregled = new RelayCommand(Execute_IzmeniPregled);

            ObrisiPregled = new RelayCommand(Execute_ObrisiPregled);

            VidiPacijentaPregleda = new RelayCommand(Execute_PrikaziPacijentaPregleda);

            IzmeniOperaciju = new RelayCommand(Execute_IzmeniOperaciju);

            ObrisiOperaciju = new RelayCommand(Execute_ObrisiOperaciju);

            VidiPacijentaOperacije = new RelayCommand(Execute_PrikaziPacijentaOperacije);

            foreach (Operacija opIter in operacijaKontroler.GetSveBuduceOperacijeLekara(jmbgLekara))
            {
                operacije.Add(opIter);
            }

            foreach (Pregled prIter in pregledKontroler.GetSviBuduciPreglediLekara(jmbgLekara))
            {
                pregledi.Add(prIter);
            }
            
        }

    }
}
