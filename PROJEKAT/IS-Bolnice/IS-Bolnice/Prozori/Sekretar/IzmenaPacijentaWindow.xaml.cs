using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Baze;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class IzmenaPacijentaWindow : Window
    {
        private Pacijent pacijentZaIzmenu;   // potrebno za izmenu pacijenta da bi se prosledio stari JMBG
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private KorisnikKontroler korisnikKontroler = new KorisnikKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private IzmenaTerminaFajlRepozitorijum bazaIzmena = new IzmenaTerminaFajlRepozitorijum();
        private List<Lekar> lekari;
        private ObservableCollection<Pacijent> PacijentiRef;
        private IzmenaTerminaKontroler izmenaTerminaKontroler = new IzmenaTerminaKontroler();

        public IzmenaPacijentaWindow(Pacijent p, ObservableCollection<Pacijent> Pacijenti)
        {
            InitializeComponent();

            pacijentZaIzmenu = p;
            lekari = lekarKontroler.GetSviLekariOpstePrakse();  // samo lekari opste prakse mogu biti izabrani lekari
            PacijentiRef = Pacijenti;
            txtJMBG.IsEnabled = false;
            PopunjvanjePoljaZaPrikaz();
        }

        private void PopunjvanjePoljaZaPrikaz()
        {
            txtIme.Text = pacijentZaIzmenu.Ime;
            txtPrezime.Text = pacijentZaIzmenu.Prezime;
            txtJMBG.Text = pacijentZaIzmenu.Jmbg;
            txtAdresa.Text = pacijentZaIzmenu.Adresa;
            txtBrTelefona.Text = pacijentZaIzmenu.BrojTelefona;
            txtEMail.Text = pacijentZaIzmenu.EMail;
            OznaciPol();
            datum.SelectedDate = pacijentZaIzmenu.DatumRodjenja;
            OznaciIzabranogLekara();
            txtKorisnickoIme.Text = pacijentZaIzmenu.KorisnickoIme;
            txtLozinka.Password = pacijentZaIzmenu.Sifra;
            // ako je blokiran cekira se check box
            if (izmenaTerminaKontroler.DaLiJePacijentMaliciozan(pacijentZaIzmenu))
            {
                CbBlokiran.IsChecked = true;
            }
        }

        private void OznaciPol()
        {
            if (pacijentZaIzmenu.Pol.ToString().Equals("muski"))
            {
                comboPol.SelectedIndex = 0;
            }
            else if (pacijentZaIzmenu.Pol.ToString().Equals("zenski"))
            {
                comboPol.SelectedIndex = 1;
            }
            else
            {
                comboPol.SelectedIndex = 2;
            }
        }

        private void OznaciIzabranogLekara()
        {
            comboLekari.ItemsSource = FormirajStringZaSvakogLekara();
            if (pacijentZaIzmenu.IzabraniLekar == null)
            {
                comboLekari.SelectedIndex = -1;
            }
            else
            {
                int indeks = 0;
                foreach (Lekar l in lekari)
                {
                    if (l.Jmbg.Equals(pacijentZaIzmenu.IzabraniLekar.Jmbg))
                    {
                        break;
                    }
                    indeks++;
                }
                comboLekari.SelectedIndex = indeks;
            }
        }

        private List<string> FormirajStringZaSvakogLekara()
        {
            List<string> lekariString = new List<string>();
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Oblast.Naziv + ")";
                lekariString.Add(lekarString);
            }

            return lekariString;
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
                Pol tempPol = OdrediPol();
                DateTime tempDatumRodjenja = (DateTime)datum.SelectedDate;
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
                    Guest = false
                };

                pacijentKontroler.IzmeniPacijenta(p);
                OsvezavanjeListePacijenata(p);
                // ako nije oznaceno da je blokiran poziva se metoda za odblokiranje
                if (!(bool)CbBlokiran.IsChecked)
                {
                    izmenaTerminaKontroler.OdblokirajPacijenta(p);
                }

                this.Close();
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

        private void OsvezavanjeListePacijenata(Pacijent izmenjenPacijent)
        {
            int i = PacijentiRef.IndexOf(pacijentZaIzmenu);
            if (i != -1)
            {
                PacijentiRef[i] = izmenjenPacijent;
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
            string tempNoviJmbg = txtJMBG.Text;
            if (!korisnikKontroler.JedinstvenNoviJmbgKorisnika(tempNoviJmbg, pacijentZaIzmenu.Jmbg))
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
            string tempNovoKorisnickoIme = txtKorisnickoIme.Text;
            if (!korisnikKontroler.JedinstvenoNovoKorisnickoImeKroisnika(tempNovoKorisnickoIme, pacijentZaIzmenu.KorisnickoIme))
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
