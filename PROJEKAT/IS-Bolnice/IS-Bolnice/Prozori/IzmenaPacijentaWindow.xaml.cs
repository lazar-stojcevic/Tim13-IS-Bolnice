using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for IzmenaPacijentaWindow.xaml
    /// </summary>
    public partial class IzmenaPacijentaWindow : Window
    {
        private Pacijent pacijent;   // potrebno za izmenu pacijenta da bi se prosledio stari JMBG
        private BazaPacijenata bp;
        private BazaLekara bl;
        private List<Lekar> lekari;
        private ObservableCollection<Pacijent> PacijentiRef;

        public IzmenaPacijentaWindow(Pacijent p, ObservableCollection<Pacijent> Pacijenti)
        {
            InitializeComponent();

            pacijent = p;
            bp = new BazaPacijenata();
            bl = new BazaLekara();
            lekari = bl.SviLekari();
            PacijentiRef = Pacijenti;

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

            datum.SelectedDate = p.DatumRodjenja;

            // oznacavanje izabranog lekara
            List<string> lekariString = new List<string>();

            // formiranje stringa za svakog lekara
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Tip + ")";
                lekariString.Add(lekarString);
            }

            // postavljanje combo boxa na odgovarajuceg lekara
            comboLekari.ItemsSource = lekariString;
            if (p.IzabraniLekar == null)
            {
                comboLekari.SelectedIndex = -1;
            }
            else
            {
                int indeks = 0;
                foreach (Lekar l in lekari)
                {
                    if (l.Jmbg.Equals(p.IzabraniLekar.Jmbg))
                    {
                        break;
                    }
                    indeks++;
                }
                comboLekari.SelectedIndex = indeks;
            }

            txtKorisnickoIme.Text = p.KorisnickoIme;
            txtLozinka.Password = p.Sifra;
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
                DateTime tempDatumRodjenja = (DateTime)datum.SelectedDate;

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
                    IzabraniLekar = lekar
                };

                // p je pacijent sa izmenjenim informacijama, a "pacijent" predstavlja selektovanog pacijenta (bitno ukoliko ima potreba da se promeni JMBG)
                bp.IzmeniPacijenta(p, pacijent);
                // osvezavanje liste
                int i = PacijentiRef.IndexOf(pacijent);
                if (i != -1)
                {
                    PacijentiRef[i] = p;
                }

                this.Close();
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                // potrebno je omoguciti da ostane isti ako se JMBG ne menja
                if (p.Jmbg.Equals(tempJmbg) && !p.Jmbg.Equals(pacijent.Jmbg))
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
                // potrebno je omoguciti da ostane isti ako se JMBG ne menja
                if (p.KorisnickoIme.Equals(tempKorisnickoIme) && !p.KorisnickoIme.Equals(pacijent.KorisnickoIme))
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
