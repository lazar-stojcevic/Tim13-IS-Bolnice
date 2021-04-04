using System.Collections.ObjectModel;
using System.Windows;


namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for DodavanjeGuestNalogaWindow.xaml
    /// </summary>
    public partial class DodavanjeGuestNalogaWindow : Window
    {
        private BazaPacijenata bp;
        private ObservableCollection<Pacijent> PacijentiRef;

        public DodavanjeGuestNalogaWindow(ObservableCollection<Pacijent> Pacijenti)
        {
            InitializeComponent();

            bp = new BazaPacijenata();
            PacijentiRef = Pacijenti;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            string tempIme = txtIme.Text;
            string tempPrezime = txtPrezime.Text;
            string tempJmbg = txtJMBG.Text;

            Pacijent p = new Pacijent
            {
                Jmbg = tempJmbg,
                KorisnickoIme = "",
                Sifra = "",
                Ime = tempIme,
                Prezime = tempPrezime,
                BrojTelefona = "",
                EMail = "",
                Adresa = "",
                Pol = Pol.drugo,
                IzabraniLekar = null,
                Guest = true
            };

            bp.KreirajPacijenta(p);
            PacijentiRef.Add(p);

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
