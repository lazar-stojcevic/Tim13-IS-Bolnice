using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.ViewModel.Sekretar
{
    class AzuriranjeAlergenaViewModel : ViewModel
    {
        #region Polja

        private Window window;
        private Pacijent pacijentRef;
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private SastojakKontroler sastojakKontroler = new SastojakKontroler();
        private string pacijentTxt;
        private ObservableCollection<string> moguciAlergeniZaDodavanje;
        private ObservableCollection<Sastojak> alergeniPacijenta;
        private string selektovaniAlergenDodavanje;
        private Sastojak selektovaniAlergenUklanjanje;

        public ObservableCollection<string> MoguciAlergeniZaDodavanje
        {
            get { return moguciAlergeniZaDodavanje; }
            set
            {
                moguciAlergeniZaDodavanje = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Sastojak> AlergeniPacijenta
        {
            get { return alergeniPacijenta; }
            set
            {
                alergeniPacijenta = value;
                OnPropertyChanged();
            }
        }

        public string PacijentTxt
        {
            get { return pacijentTxt; }
            set
            {
                pacijentTxt = value;
                OnPropertyChanged();
            }
        }

        public string SelektovaniAlergenDodavanje
        {
            get { return selektovaniAlergenDodavanje; }
            set
            {
                selektovaniAlergenDodavanje = value;
                OnPropertyChanged();
            }
        }

        public Sastojak SelektovaniAlergenUklanjanje
        {
            get { return selektovaniAlergenUklanjanje; }
            set
            {
                selektovaniAlergenUklanjanje = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Komande

        private RelayCommand dodajCommand;
        private RelayCommand ukloniCommand;
        private RelayCommand potvrdiCommand;
        private RelayCommand odustaniCommand;

        public RelayCommand DodajCommand
        {
            get { return dodajCommand; }
            set
            {
                dodajCommand = value;
            }
        }

        public RelayCommand UkloniCommand
        {
            get { return ukloniCommand; }
            set
            {
                ukloniCommand = value;
            }
        }

        public RelayCommand PotvrdiCommand
        {
            get { return potvrdiCommand; }
            set
            {
                potvrdiCommand = value;
            }
        }

        public RelayCommand OdustaniCommand
        {
            get { return odustaniCommand; }
            set
            {
                odustaniCommand = value;
            }
        }

        public void Execute_DodajCommand(object obj)
        {
            if (selektovaniAlergenDodavanje != null)
            {
                alergeniPacijenta.Add(new Sastojak(selektovaniAlergenDodavanje));
            }
            AlergeniPacijenta = alergeniPacijenta;
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        public void Execute_UkloniCommand(object obj)
        {
            if (selektovaniAlergenUklanjanje != null)
            {
                alergeniPacijenta.Remove(selektovaniAlergenUklanjanje);
            }

            AlergeniPacijenta = alergeniPacijenta;
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        public void Execute_PotvrdiCommand(object obj)
        {
            pacijentRef.Alergeni = alergeniPacijenta.ToList();
            pacijentKontroler.IzmeniPacijenta(pacijentRef);
            window.Close();
        }

        public void Execute_OdustaniCommand(object obj)
        {
            window.Close();
        }

        #endregion

        private void AzuriranjeMogucihAlergenaZaDodavanje()
        {
            List<Sastojak> sviSastojci = sastojakKontroler.GetSviSastojci();
            moguciAlergeniZaDodavanje.Clear();
            foreach (Sastojak sastojak in sviSastojci)
            {
                if (!PacijentPosedujeAlergen(sastojak))
                {
                    moguciAlergeniZaDodavanje.Add(sastojak.Ime);
                }
            }

            MoguciAlergeniZaDodavanje = moguciAlergeniZaDodavanje;
        }

        private bool PacijentPosedujeAlergen(Sastojak sastojak)
        {
            foreach (Sastojak pacijentovAlergen in alergeniPacijenta)
            {
                if (sastojak.Isti(pacijentovAlergen))
                {
                    return true;
                }
            }
            return false;
        }

        public AzuriranjeAlergenaViewModel(Window prosledjenWindow, Pacijent pacijent)
        {
            window = prosledjenWindow;
            pacijentRef = pacijent;
            alergeniPacijenta = new ObservableCollection<Sastojak>(pacijent.Alergeni);
            moguciAlergeniZaDodavanje = new ObservableCollection<string>();
            this.PacijentTxt = pacijent.Ime + " " + pacijent.Prezime;
            AzuriranjeMogucihAlergenaZaDodavanje();
            DodajCommand = new RelayCommand(Execute_DodajCommand);
            UkloniCommand = new RelayCommand(Execute_UkloniCommand);
            PotvrdiCommand = new RelayCommand(Execute_PotvrdiCommand);
            OdustaniCommand = new RelayCommand(Execute_OdustaniCommand);
        }
    }
}
