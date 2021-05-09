using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class PrikazSvihPacijenata : Window
    {
        private BazaPacijenata bp;
        private Pacijent odabraniPacijentRef;
        private ObservableCollection<Pacijent> odabraniPacijentiRef;
        public ObservableCollection<Pacijent> Pacijenti
        {
            get;
            set;
        }

        public PrikazSvihPacijenata(Pacijent pacijent)
        {
            InitializeComponent();

            this.DataContext = this;
            bp = new BazaPacijenata();
            Pacijenti = new ObservableCollection<Pacijent>(bp.SviPacijenti());
            odabraniPacijentRef = pacijent;

            dgPacijenti.SelectionMode = DataGridSelectionMode.Single;
            pomocniTekst.Content = "";
        }

        // ovaj konstruktor se koristi iz prikaza za obavestenja
        public PrikazSvihPacijenata(ObservableCollection<Pacijent> odabraniPacijenti)
        {
            InitializeComponent();

            this.DataContext = this;
            bp = new BazaPacijenata();
            Pacijenti = new ObservableCollection<Pacijent>(bp.SviPacijenti());

            odabraniPacijentiRef = odabraniPacijenti;
        }

        private bool SadrziPacijenta(List<Pacijent> pacijenti, Pacijent pacijent)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.Jmbg.Equals(pacijent.Jmbg))
                {
                    return true;
                }
            }
            return false;
        }

        private void BelezenjeVisePacijenata()
        {
            for (int i = 0; i < dgPacijenti.SelectedItems.Count; i++)
            {
                Pacijent pacijent = (Pacijent)dgPacijenti.SelectedItems[i];
                if (!SadrziPacijenta(odabraniPacijentiRef.ToList(), pacijent))
                {
                    odabraniPacijentiRef.Add(pacijent);
                }
            }
        }

        private void BelezenjeJednogPacijenta()
        {
            Pacijent pacijent = (Pacijent)dgPacijenti.SelectedItem;
            // menjanje objekta uz pomoc kopije prosledjene reference
            odabraniPacijentRef.Jmbg = pacijent.Jmbg;
            odabraniPacijentRef.KorisnickoIme = pacijent.KorisnickoIme;
            odabraniPacijentRef.Sifra = pacijent.Sifra;
            odabraniPacijentRef.Ime = pacijent.Ime;
            odabraniPacijentRef.Prezime = pacijent.Prezime;
            odabraniPacijentRef.BrojTelefona = pacijent.BrojTelefona;
            odabraniPacijentRef.EMail = pacijent.EMail;
            odabraniPacijentRef.Adresa = pacijent.Adresa;
            odabraniPacijentRef.Pol = pacijent.Pol;
            odabraniPacijentRef.DatumRodjenja = pacijent.DatumRodjenja;
            odabraniPacijentRef.IzabraniLekar = pacijent.IzabraniLekar;
            odabraniPacijentRef.Alergeni = pacijent.Alergeni;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (odabraniPacijentRef != null)
            {
                BelezenjeJednogPacijenta();
            }
            else if (odabraniPacijentiRef != null)
            {
                BelezenjeVisePacijenata();
            }

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
