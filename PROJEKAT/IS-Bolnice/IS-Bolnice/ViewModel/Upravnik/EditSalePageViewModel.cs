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
    class EditSalePageViewModel: ViewModel
    {
        #region Polja
        BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        WPFNavigacijaKontroler navigacijaKontroler;
        private Soba izmenjenaSoba;
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

        public Soba IzmenjenaSoba
        {
            get { return izmenjenaSoba; }
            set {
                izmenjenaSoba = value;
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
        #endregion

        #region Komande
        private RelayCommand editCommand;

        private RelayCommand cancelEditCommand;

        private RelayCommand deleteCommand;

        public RelayCommand EditCommand
        {
            get { return editCommand; }
            set
            {
                editCommand = value;
            }
        }

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                deleteCommand = value;
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

        public void Executed_EditCommand(object obj)
        {
            IzmenjenaSoba.Id = this.Id;
            IzmenjenaSoba.Tip = this.Tip;
            IzmenjenaSoba.Kvadratura = this.Kvadratura;
            IzmenjenaSoba.Sprat = this.Sprat;
            bolnicaKontroler.IzmeniSobu(IzmenjenaSoba);
            Execute_CancelEditCommand(null);
        }

        public void Execute_CancelEditCommand(object obj)
        {
            navigacijaKontroler.PrikaziSalePage();
        }

        public void Execute_DeleteCommand(object obj)
        {
            bolnicaKontroler.ObrisiSobu(this.Id);
            Execute_CancelEditCommand(null);
        }
        #endregion

        public EditSalePageViewModel(string id, NavigationService navService)
        {
            NavigacijaKontroler = new WPFNavigacijaKontroler(navService);
            izmenjenaSoba = bolnicaKontroler.GetSobaPoId(id);
            this.Id = izmenjenaSoba.Id;
            this.Kvadratura = izmenjenaSoba.Kvadratura;
            this.Sprat = izmenjenaSoba.Sprat;
            this.Tip = izmenjenaSoba.Tip;
            EditCommand = new RelayCommand(Executed_EditCommand);
            DeleteCommand = new RelayCommand(Execute_DeleteCommand);
            CancelEditCommand = new RelayCommand(Execute_CancelEditCommand);
            DodajTipove();
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
