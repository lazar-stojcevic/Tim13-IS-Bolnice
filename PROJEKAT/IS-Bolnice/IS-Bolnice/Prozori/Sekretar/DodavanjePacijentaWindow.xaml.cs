using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
                InformativniProzor ip = new InformativniProzor("Nisu popunjena sva obavezna polja!");
                ip.ShowDialog();
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
                DateTime tempDatumRodjenja = OdrediDatumRodjenja();
                Pol tempPol = OdrediPol();
                Lekar lekar = OdrediIzabranogLekara();

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

        private DateTime OdrediDatumRodjenja()
        {
            if (datum.SelectedDate == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return (DateTime)datum.SelectedDate;
            }
        }

        private Pol OdrediPol()
        {
            string polString = comboPol.Text;
            if (polString.Equals("Muški"))
            {
                return Pol.muski;
            }
            else if (polString.Equals("Ženski"))
            {
                return Pol.zenski;
            }
            else
            {
                return Pol.drugo;
            }
        }

        private Lekar OdrediIzabranogLekara()
        {
            int indeks = comboLekari.SelectedIndex;
            if (indeks == -1)
            {
                return null;
            }
            else
            {
                return lekari[indeks];
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
                InformativniProzor ip = new InformativniProzor("Uneti JMBG već postoji u sistemu!");
                ip.ShowDialog();
            }

            if (!Regex.IsMatch(tempJmbg, "^[0-9]{13}$") && tempJmbg != "")
            {
                txtJMBG.Text = "";
                InformativniProzor ip = new InformativniProzor("JMBG se mora sastojati od 13 cifara.");
                ip.ShowDialog();
            }
        }

        private void TxtEMail_OnLostFocus(object sender, RoutedEventArgs e)
        {
            string temlMail = txtEMail.Text;
            if (!Regex.IsMatch(temlMail, @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$") && temlMail != "")
            {
                txtEMail.Text = "";
                InformativniProzor ip = new InformativniProzor("Email adresa nije u validnom formatu");
                ip.ShowDialog();
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
                InformativniProzor ip = new InformativniProzor("Uneto korisničko ime već postoji u sistemu!");
                ip.ShowDialog();
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
