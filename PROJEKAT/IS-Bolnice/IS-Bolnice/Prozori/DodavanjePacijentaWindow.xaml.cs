using System.Windows;


namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for DodavanjePacijentaWindow.xaml
    /// </summary>
    public partial class DodavanjePacijentaWindow : Window
    {
        BazaPacijenata bp;

        public DodavanjePacijentaWindow()
        {
            InitializeComponent();

            bp = new BazaPacijenata();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            string tempIme = txtIme.Text;
            string tempPrezime = txtPrezime.Text;
            string tempJmbg = txtJMBG.Text;
            string tempAdresa = txtAdresa.Text;
            string tempBrTelefona = txtBrTelefona.Text;
            string tempEMail = txtEMail.Text;
            string tempKorisnickoIme = txtKorisnickoIme.Text;
            string tempLozinka = txtLozinka.Password;

            string polString = comboPol.Text;
            Pol tempPol;
            
            if (polString.Equals("Muški"))
            {
                tempPol = Pol.muski;
            } 
            else if (polString.Equals("Ženski"))
            {
                tempPol = Pol.zenski;
            } 
            else
            {
                tempPol = Pol.drugo;
            }

            Pacijent p = new Pacijent
            {
                Jmbg = tempJmbg,
                KorisnickoIme = tempKorisnickoIme,
                Sifra = tempLozinka,
                Ime = tempIme,
                Prezime = tempPrezime,
                BrojTelefona = tempBrTelefona,
                EMail = tempEMail,
                Adresa = tempAdresa,
                Pol = tempPol,
                IzabraniLekar = null
            };

            bp.KreirajPacijenta(p);

            this.Close();
        }
    }
}
