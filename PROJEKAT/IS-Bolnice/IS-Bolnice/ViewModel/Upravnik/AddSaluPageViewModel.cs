using IS_Bolnice.Kontroleri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace IS_Bolnice.ViewModel.Upravnik
{
    class AddSaluPageViewModel: ViewModel
    {
        BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        WPFNavigacijaKontroler navigacijaKontroler;
        private Soba novaSoba;
        private string id;
        private RoomType tip;
        private int sprat;
        private double kvadratura;
        List<RoomType> sviMoguciTipovi = new List<RoomType>();

        public WPFNavigacijaKontroler NavigacijaKontroler
        {
            get { return navigacijaKontroler; }
            set
            {
                navigacijaKontroler = value;
            }
        }

        public List<RoomType> SviMoguciTipovi
        {
            get { return sviMoguciTipovi; }
            set
            {
                sviMoguciTipovi = value;
            }
        }

        public Soba NovaSoba
        {
            get { return novaSoba; }
            set
            {
                novaSoba = value;
            }
        }
        public string Id
        {
            get { return id; }
            set
            {
                if (value.Contains("#") || value.Contains("/"))
                {
                    CustomMessageBox.ShowOK("Podaci nisu validno uneti! Ne sme biti #!", "Greška", "Potvrdi");
                }
                else
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }
        public RoomType Tip
        {
            get { return tip; }
            set
            {
                tip = value;
                OnPropertyChanged();
            }
        }


        public int Sprat
        {
            get { return sprat; }
            set
            {
                sprat = value;
                OnPropertyChanged();
            }
        }
        public double Kvadratura
        {
            get { return kvadratura; }
            set
            {
                kvadratura = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand addCommand;

        private RelayCommand cancelEditCommand;

        public RelayCommand AddCommand
        {
            get { return addCommand; }
            set
            {
                addCommand = value;
            }
        }


        public RelayCommand CancelEditCommand
        {
            get { return cancelEditCommand; }
            set
            {
                cancelEditCommand = value;
            }
        }

        public void Executed_AddCommand(object obj)
        {
            novaSoba.Id = this.Id;
            novaSoba.Tip = this.Tip;
            novaSoba.Kvadratura = this.Kvadratura;
            novaSoba.Sprat = this.Sprat;
            bolnicaKontroler.KreirajSobuUBolnici(novaSoba);
            Execute_CancelEditCommand(null);
        }

        public void Execute_CancelEditCommand(object obj)
        {
            navigacijaKontroler.PrikaziSalePage();
        }

        public AddSaluPageViewModel(NavigationService navService)
        {
            NavigacijaKontroler = new WPFNavigacijaKontroler(navService);
            AddCommand = new RelayCommand(Executed_AddCommand);
            CancelEditCommand = new RelayCommand(Execute_CancelEditCommand);
            PostaviPolja();
            DodajTipove();
        }

        private void PostaviPolja()
        {
            NovaSoba = new Soba();
            this.Id = "0";
            this.Kvadratura = 0;
            this.Sprat = 0;
            this.Tip = RoomType.bolnickaSoba;
        }
        private void DodajTipove()
        {
            sviMoguciTipovi.Add(RoomType.bolnickaSoba);
            sviMoguciTipovi.Add(RoomType.magacin);
            sviMoguciTipovi.Add(RoomType.operacionaSala);
            sviMoguciTipovi.Add(RoomType.ordinacija);
        }
    }
}
