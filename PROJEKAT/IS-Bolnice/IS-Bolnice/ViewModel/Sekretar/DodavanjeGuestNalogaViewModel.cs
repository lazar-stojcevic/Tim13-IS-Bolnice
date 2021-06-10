using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Sekretar;

namespace IS_Bolnice.ViewModel.Sekretar
{
    class DodavanjeGuestNalogaViewModel : ViewModel
    {
        #region Polja

        private Window window;
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private KorisnikKontroler korisnikKontroler = new KorisnikKontroler();
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
            if (MozeDaSeKreira())
            {
                Pacijent noviGuestPacijent = new Pacijent(jmbg, ime, prezime);
                noviGuestPacijent.Guest = true;
                pacijentKontroler.KreirajPacijenta(noviGuestPacijent);
                window.Close();
            }
        }

        public void Execute_OdustaniCommand(object obj)
        {
            window.Close();
        }

        #endregion


        private bool PopunjenaObaveznaPolja()
        {
            if (jmbg != null)
            {
                return true;
            }

            return false;
        }

        private bool MozeDaSeKreira()
        {
            if (!PopunjenaObaveznaPolja())
            {
                InformativniProzor ip = new InformativniProzor("Morate popuniti obavezna polja.");
                ip.Show();
                return false;
            }
            if (!Regex.IsMatch(jmbg, "^[0-9]{13}$"))
            {
                Jmbg = "";
                InformativniProzor ip = new InformativniProzor("JMBG se mora sastojati od 13 cifara.");
                ip.ShowDialog();
                return false;
            }
            if (!korisnikKontroler.JedinstvenJmbgKorisnika(jmbg))
            {
                InformativniProzor ip = new InformativniProzor("Uneti JMBG već postoji u sistemu!");
                ip.ShowDialog();
                return false;
            }

            return true;
        }

        public DodavanjeGuestNalogaViewModel(Window prosledjenWindow)
        {
            window = prosledjenWindow;
            PotvrdiCommand = new RelayCommand(Execute_PotvrdiCommand);
            OdustaniCommand = new RelayCommand(Execute_OdustaniCommand);
        }
    }
}
