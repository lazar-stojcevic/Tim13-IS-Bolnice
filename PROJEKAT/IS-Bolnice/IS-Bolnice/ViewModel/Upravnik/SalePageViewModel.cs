using IS_Bolnice.Kontroleri;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace IS_Bolnice.ViewModel.Upravnik
{
    class SalePageViewModel: ViewModel
    {
        private BolnicaKontroler bolnicaKontroler;
        private ObservableCollection<Soba> sobe;
        private WPFNavigacijaKontroler navigacijaKontroler;
        private Soba selektovanaSoba;
        private List<string> mogucaFiltriranja = new List<string>();
        private string selektovaniFilter;

        public WPFNavigacijaKontroler NavigacijaKontroler
        {
            get { return navigacijaKontroler; }
            set
            {
                navigacijaKontroler = value;
            }
        }

        public ObservableCollection<Soba> Sobe
        {
            get { return sobe; }
            set
            {
                sobe = value;
                OnPropertyChanged();
            }
        }

        public List<string> MogucaFiltriranja
        {
            get { return mogucaFiltriranja; }
            set
            {
                mogucaFiltriranja = value;
            }
        }

        public string SelektovaniFilter
        {
            get { return selektovaniFilter; }
            set
            {
                selektovaniFilter = value;
                Executed_SelectedCommand(null);
                OnPropertyChanged();
            }
        }

        public Soba SelektovanaSoba
        {
            get { return selektovanaSoba; }
            set
            {
                selektovanaSoba = value;
                Executed_SelectedSobaCommand(null);
                OnPropertyChanged();
            }
        }


        private RelayCommand addCommand;

        private RelayCommand sveSelectedCommand;
        private RelayCommand bolnickaSobaSelectedCommand;
        private RelayCommand ordinacijaSelectedCommand;
        private RelayCommand operacionaSalaSelectedCommand;
        private RelayCommand magaciniSelectedCommand;
        private RelayCommand selectedCommand;

        private RelayCommand selectedSobaCommand;

        public RelayCommand SelectedSobaCommand
        {
            get { return selectedSobaCommand; }
            set
            {
                selectedSobaCommand = value;
            }
        }

        public RelayCommand SelectedCommand
        {
            get { return selectedCommand; }
            set
            {
                selectedCommand = value;
            }
        }

        public RelayCommand AddCommand
        {
            get { return addCommand; }
            set
            {
                addCommand = value;
            }
        }

        public RelayCommand BolnickaSobaSelectedCommand
        {
            get { return bolnickaSobaSelectedCommand; }
            set
            {
                bolnickaSobaSelectedCommand = value;
            }
        }

        public RelayCommand OrdinacijaSelectedCommand
        {
            get { return ordinacijaSelectedCommand; }
            set
            {
                ordinacijaSelectedCommand = value;
            }
        }

        public RelayCommand SveSelectedCommand
        {
            get { return sveSelectedCommand; }
            set
            {
                sveSelectedCommand = value;
            }
        }

        public RelayCommand OperacionaSalaSelectedCommand
        {
            get { return operacionaSalaSelectedCommand; }
            set
            {
                operacionaSalaSelectedCommand = value;
            }
        }

        public RelayCommand MagaciniSelectedCommand
        {
            get { return magaciniSelectedCommand; }
            set
            {
                magaciniSelectedCommand = value;
            }
        }

        public void Executed_AddCommand(object obj)
        {
            NavigacijaKontroler.PrikaziAddSalePage();
        }

        public void Executed_SelectedSobaCommand(object obj)
        {
            NavigacijaKontroler.PrikaziEditSalePage(selektovanaSoba.Id);
        }

        public void Executed_SelectedCommand(object obj)
        {
            switch (SelektovaniFilter) {
                case "Sve prostorije":
                    Executed_SveSelectedCommand(null);
                    break;
                case "Bolnicke sobe":
                    Executed_BolnickaSobaSelectedCommand(null);
                    break;
                case "Operacione sale":
                    Executed_OperacionaSalaSelectedCommand(null);
                    break;
                case "Ordinacije":
                    Executed_OrdinacijaSelectedCommand(null);
                    break;
                default:
                    Executed_MagaciniSelectedCommand(null);
                    break;
            }
        }

        public void Executed_BolnickaSobaSelectedCommand(object obj)
        {
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe.Clear();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false && soba.Tip == RoomType.bolnickaSoba)
                {
                    sobe.Add(soba);
                }
            }
        }

        public void Executed_OrdinacijaSelectedCommand(object obj)
        {
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe.Clear();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false && soba.Tip == RoomType.ordinacija)
                {
                    sobe.Add(soba);
                }
            }
        }

        public void Executed_SveSelectedCommand(object obj)
        {
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe.Clear();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false)
                {
                    sobe.Add(soba);
                }
            }
        }

        public void Executed_OperacionaSalaSelectedCommand(object obj)
        {
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe.Clear();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false && soba.Tip == RoomType.operacionaSala)
                {
                    sobe.Add(soba);
                }
            }
        }

        public void Executed_MagaciniSelectedCommand(object obj)
        {
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe.Clear();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false && soba.Tip == RoomType.magacin)
                {
                    sobe.Add(soba);
                }
            }
        }

        public SalePageViewModel(NavigationService navService)
        {
            NavigacijaKontroler = new WPFNavigacijaKontroler(navService);
            bolnicaKontroler = new BolnicaKontroler();
            List<Soba> prostrorije = bolnicaKontroler.GetSveSobe();
            sobe = new ObservableCollection<Soba>();
            foreach (Soba soba in prostrorije)
            {
                if (soba.Obrisano == false)
                {
                    sobe.Add(soba);
                }
            }
            NapraviKomande();
            DodajMogucaFiltriranja();
            SelektovaniFilter = "Sve prostorije";
        }

        private void NapraviKomande()
        {
            AddCommand = new RelayCommand(Executed_AddCommand);
            SveSelectedCommand = new RelayCommand(Executed_SveSelectedCommand);
            BolnickaSobaSelectedCommand = new RelayCommand(Executed_BolnickaSobaSelectedCommand);
            OperacionaSalaSelectedCommand = new RelayCommand(Executed_OperacionaSalaSelectedCommand);
            OrdinacijaSelectedCommand = new RelayCommand(Executed_OrdinacijaSelectedCommand);
            MagaciniSelectedCommand = new RelayCommand(Executed_MagaciniSelectedCommand);
            SelectedSobaCommand = new RelayCommand(Executed_SelectedSobaCommand);
        }

        private void DodajMogucaFiltriranja()
        {
            mogucaFiltriranja.Add("Sve prostorije");
            mogucaFiltriranja.Add("Bolnicke sobe");
            mogucaFiltriranja.Add("Operacione sale");
            mogucaFiltriranja.Add("Ordinacije");
            mogucaFiltriranja.Add("Magacini");
        }

    }
}
