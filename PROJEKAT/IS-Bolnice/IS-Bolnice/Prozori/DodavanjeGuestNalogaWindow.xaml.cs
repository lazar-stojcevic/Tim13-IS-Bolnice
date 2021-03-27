using System.Windows;


namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for DodavanjeGuestNalogaWindow.xaml
    /// </summary>
    public partial class DodavanjeGuestNalogaWindow : Window
    {
        BazaPacijenata bp;

        public DodavanjeGuestNalogaWindow()
        {
            InitializeComponent();

            bp = new BazaPacijenata();
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

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
