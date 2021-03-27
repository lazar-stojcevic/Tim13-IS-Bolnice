using System.Windows;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for IzmenaPacijentaWindow.xaml
    /// </summary>
    public partial class IzmenaPacijentaWindow : Window
    {
        Pacijent pacijent;   // potrebno za izmenu pacijenta da bi se prosledio stari JMBG
        BazaPacijenata bp;

        public IzmenaPacijentaWindow(Pacijent p)
        {
            InitializeComponent();

            pacijent = p;
            bp = new BazaPacijenata();

            txtIme.Text = p.Ime;
            txtPrezime.Text = p.Prezime;
            txtJMBG.Text = p.Jmbg;
            txtAdresa.Text = p.Adresa;
            txtBrTelefona.Text = p.BrojTelefona;
            txtEMail.Text = p.EMail;
            if (p.Pol.ToString().Equals("muski"))
            {
                comboPol.SelectedIndex = 0;
            }
            else if (p.Pol.ToString().Equals("zenski"))
            {
                comboPol.SelectedIndex = 1;
            }
            else
            {
                comboPol.SelectedIndex = 2;
            }
            txtKorisnickoIme.Text = p.KorisnickoIme;
            txtLozinka.Password = p.Sifra;
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

            // p je pacijent sa izmenjenim informacijama, a "pacijent" predstavlja selektovanog pacijenta (bitno ukoliko ima potreba da se promeni JMBG)
            bp.IzmeniPacijenta(p, pacijent);

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
