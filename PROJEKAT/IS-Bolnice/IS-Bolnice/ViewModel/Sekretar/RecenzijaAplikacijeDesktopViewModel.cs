using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Model;
using IS_Bolnice.Prozori.Sekretar;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.ViewModel.Sekretar
{
    class RecenzijaAplikacijeDesktopViewModel : ViewModel
    {
        #region Polja

        private Window window;
        private List<string> ponudjeneOcene = new List<string>();
        private string selektovanaOcena;
        private string opis;

        public List<string> PonudjeneOcene
        {
            get { return ponudjeneOcene; }
            set
            {
                ponudjeneOcene = value;
                OnPropertyChanged();
            }
        }

        public string SelektovanaOcena
        {
            get { return selektovanaOcena; }
            set
            {
                selektovanaOcena = value;
                OnPropertyChanged();
            }
        }

        public string Opis
        {
            get { return opis; }
            set
            {
                opis = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Komande

        private RelayCommand potvrdiCommand;
        private RelayCommand odustaniCommand;

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

        public void Execute_PotvrdiCommand(object obj)
        {
            try
            {
                Recenzija recenzija = new Recenzija();
                recenzija.Ocena = Int16.Parse(selektovanaOcena);
                recenzija.Opis = opis;
                new RecenzijaServis().KreirajRecenziju(recenzija);
                InformativniProzor ip = new InformativniProzor("Uspešno ste poslali recenziju.");
                ip.ShowDialog();
                window.Close();
            }
            catch
            {
                InformativniProzor ip = new InformativniProzor("Niste selektovali ocenu.");
                ip.ShowDialog();
            }
        }

        public void Execute_OdustaniCommand(object obj)
        {
            window.Close();
        }

        #endregion

        private void NamestiPonudjeneOcene()
        {
            for (int i = 1; i <= 5; i++)
            {
                ponudjeneOcene.Add(i.ToString());
            }

            PonudjeneOcene = ponudjeneOcene;
        }

        public RecenzijaAplikacijeDesktopViewModel(Window prosledjeniWindow)
        {
            window = prosledjeniWindow;
            PotvrdiCommand = new RelayCommand(Execute_PotvrdiCommand);
            OdustaniCommand = new RelayCommand(Execute_OdustaniCommand);
            PonudjeneOcene = new List<string>();
            NamestiPonudjeneOcene();
        }
    }
}
