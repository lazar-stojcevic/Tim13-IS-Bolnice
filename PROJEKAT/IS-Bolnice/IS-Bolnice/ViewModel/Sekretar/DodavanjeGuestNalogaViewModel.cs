using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.ViewModel.Sekretar
{
    class DodavanjeGuestNalogaViewModel : ViewModel
    {
        private Window window;
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private RelayCommand potvrdiCommand;
        private RelayCommand odustaniCommand;
        private string ime;
        private string prezime;
        private string jmbg;

        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged();
            }
        }

        public string Prezime
        {
            get { return prezime; }
            set
            {
                prezime = value;
                OnPropertyChanged();
            }
        }

        public string Jmbg
        {
            get { return jmbg; }
            set
            {
                jmbg = value;
                OnPropertyChanged();
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

        public DodavanjeGuestNalogaViewModel(Window prosledjenWindow)
        {
            window = prosledjenWindow;
            PotvrdiCommand = new RelayCommand(Execute_PotvrdiCommand);
            OdustaniCommand = new RelayCommand(Execute_OdustaniCommand);
        }

        public void Execute_PotvrdiCommand(object obj)
        {
            Pacijent noviGuestPacijent = new Pacijent(jmbg, ime, prezime);
            noviGuestPacijent.Guest = true;
            pacijentKontroler.KreirajPacijenta(noviGuestPacijent);
            window.Close();
        }

        public void Execute_OdustaniCommand(object obj)
        {
            window.Close();
        }
    }
}
