using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for DodavanjePacijentaWindow.xaml
    /// </summary>
    public partial class DodavanjePacijentaWindow : Window
    {
        private BazaPacijenata bp;
        private BazaLekara bl;
        private List<Lekar> lekari;

        public DodavanjePacijentaWindow()
        {
            InitializeComponent();

            bp = new BazaPacijenata();
            bl = new BazaLekara();

            List<string> lekariString = new List<string>();
            lekari = bl.LekariOpstePrakse();    // samo lekari opste prakse mogu biti izabrani lekari
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Oblast.Naziv + ")";
                lekariString.Add(lekarString);
            }
            comboLekari.ItemsSource = lekariString;
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            // ako nisu popunjena sva obavezna polja ne dozvoli dugme potvrde
            if (!popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = false;

                string sMessageBoxText = "Nisu popunjena sva obavezna polja!";
                string sCaption = "Upozorenje";

                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            else
            {
                string tempIme = txtIme.Text;
                string tempPrezime = txtPrezime.Text;
                string tempJmbg = txtJMBG.Text;
                string tempAdresa = txtAdresa.Text;
                string tempBrTelefona = txtBrTelefona.Text;
                string tempEMail = txtEMail.Text;
                string tempKorisnickoIme = txtKorisnickoIme.Text;
                string tempLozinka = txtLozinka.Password;
                DateTime tempDatumRodjenja;
                if (datum.SelectedDate == null)
                {
                    tempDatumRodjenja = DateTime.MinValue;
                }
                else
                {
                    tempDatumRodjenja = (DateTime)datum.SelectedDate;
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

                Lekar lekar;
                int indeks = comboLekari.SelectedIndex;
                if (indeks == -1)
                {
                    lekar = null;
                }
                else
                {
                    lekar = lekari[indeks];
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
                    IzabraniLekar = lekar,
                    Alergeni = new List<Sastojak>()
                };

                bp.KreirajPacijenta(p);

                this.Close();
            }
        }

        private bool popunjenaObaveznaPolja()
        {
            // provera da li su sada popunjena sva polja
            string ime = txtIme.Text;
            string prezime = txtPrezime.Text;
            string jmbg = txtJMBG.Text;
            string korisnickoIme = txtKorisnickoIme.Text;
            string lozinka = txtLozinka.Password;

            if (!(ime == "" || prezime == "" || jmbg == "" || korisnickoIme == "" || lozinka == ""))
            {
                return true;
            }

            return false;
        }

        private void txtIme_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = true;
            }
        }

        private void txtPrezime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = true;
            }
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
            if (popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = true;
            }
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
            if (popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = true;
            }
        }

        private void txtLozinka_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (popunjenaObaveznaPolja())
            {
                dugmePotvrdi.IsEnabled = true;
            }
        }
    }
}
