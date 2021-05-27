using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Servisi;


namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class DodavanjePacijentaWindow : Window
    {
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private KorisnikKontroler korisnikKontroler = new KorisnikKontroler();
        private List<Lekar> lekari;

        public DodavanjePacijentaWindow()
        {
            InitializeComponent();

            PopunjavanjeListeLekaraZaOdabir();
        }

        private void PopunjavanjeListeLekaraZaOdabir()
        {
            List<string> lekariString = new List<string>();
            lekari = lekarKontroler.GetSviLekariOpstePrakse(); // samo lekari opste prakse mogu biti izabrani lekari
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
                MessageBox.Show("Nisu popunjena sva obavezna polja!");
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

                pacijentKontroler.KreirajPacijenta(p);

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
            string tempJmbg = txtJMBG.Text;
            if (!korisnikKontroler.JedinstvenJmbgKorisnika(tempJmbg))
            {
                dugmePotvrdi.IsEnabled = false;
                MessageBox.Show("Uneti JMBG već postoji u sistemu!");
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
            string tempKorisnickoIme = txtKorisnickoIme.Text;
            if (!korisnikKontroler.JedinstvenoKorisnickoImeKorisnika(tempKorisnickoIme))
            {
                dugmePotvrdi.IsEnabled = false;
                MessageBox.Show("Uneto korisničko ime već postoji u sistemu!");
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
