using System;
using System.Collections.Generic;
using System.Windows;


namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for DodavanjePacijentaWindow.xaml
    /// </summary>
    public partial class DodavanjePacijentaWindow : Window
    {
        private BazaPacijenata bp;

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
            string tempDatumRodjenja = "";

            if (datum.IsSealed)
            {
                DateTime izabraniDatum = (DateTime)datum.SelectedDate;
                tempDatumRodjenja = izabraniDatum.ToShortDateString();
            }

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
                DatumRodjenja = tempDatumRodjenja,
                IzabraniLekar = null
            };

            bp.KreirajPacijenta(p);

            this.Close();
        }

        private void txtJMBG_LostFocus(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = bp.SviPacijenti();
            string tempJmbg = txtJMBG.Text;

            foreach (Pacijent p in pacijenti)
            {
                if (p.Jmbg.Equals(tempJmbg))
                {
                    dugmePotvrdi.IsEnabled = false;

                    string sMessageBoxText = "Uneti JMBG već postoji u sistemu!";
                    string sCaption = "Upozorenje";

                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                }
            }
        }

        private void txtJMBG_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            dugmePotvrdi.IsEnabled = true;
        }

        private void txtKorisnickoIme_LostFocus(object sender, RoutedEventArgs e)
        {
            List<Pacijent> pacijenti = bp.SviPacijenti();
            string tempKorisnickoIme = txtKorisnickoIme.Text;

            foreach (Pacijent p in pacijenti)
            {
                if (p.KorisnickoIme.Equals(tempKorisnickoIme))
                {
                    dugmePotvrdi.IsEnabled = false;

                    string sMessageBoxText = "Uneto korisničko ime već postoji u sistemu!";
                    string sCaption = "Upozorenje";

                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                }
            }
        }

        private void txtKorisnickoIme_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            dugmePotvrdi.IsEnabled = true;
        }
    }
}
