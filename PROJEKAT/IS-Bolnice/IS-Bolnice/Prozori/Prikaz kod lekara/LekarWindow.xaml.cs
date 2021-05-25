using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    
    public partial class LekarWindow : Page
    {
        public LekarWindow(string jmbgLekara)
        {
            Sifra = jmbgLekara;
            InitializeComponent();
        }

        private void btnKreirajOperaciju(object sender, RoutedEventArgs e)
        {
            var operacija = new ZakazivanjeOperacije();
            operacija.txtOperJmbg.Text = txtJMBG.Text;
            operacija.txtOperIme.Text = txtIme.Text;
            operacija.txtOperPrz.Text = txtPrz.Text;
            operacija.ShowDialog();

        }

        private void btnKreirajPregled(object sender, RoutedEventArgs e)
        {
            var pregled = new LekarZakazivanjePregleda();
            pregled.txtOperJmbg.Text = txtJMBG.Text;
            pregled.txtOperIme.Text = txtIme.Text;
            pregled.txtOperPrz.Text = txtPrz.Text;
            pregled.ShowDialog();

        }

        private void btnUcitajPacijenta(object sender, RoutedEventArgs e)
        {
            PregledKontroler kontroler = new PregledKontroler();
            Pregled pregled = kontroler.GetSledeciPregledKodLekara(Sifra);
            if (pregled.VremePocetkaPregleda.Equals(DateTime.MaxValue))
            {
                MessageBox.Show("Ne postoji ni jedan zakazan pregled kod Vas", "Ne postoji pregled",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                btnOperacija.IsEnabled = false;
                btnPregled.IsEnabled = false;
                btnIzvestaj.IsEnabled = false;
                btnHospitalizacija.IsEnabled = false;
                btnPodaci.IsEnabled = false;

            }else
            {
               //B Console.WriteLine(DateTime.MaxValue + "  AAA   " + pregled.VremePocetkaPregleda);
                txtJMBG.Text = pregled.Pacijent.Jmbg;
                txtIme.Text = pregled.Pacijent.Ime;
                txtPrz.Text = pregled.Pacijent.Prezime;
                btnOperacija.IsEnabled = true;
                btnPregled.IsEnabled = true;
                btnIzvestaj.IsEnabled = true;
                btnHospitalizacija.IsEnabled = true;
                btnPodaci.IsEnabled = true;
            }

            /*
            bool nasao = false;
            PacijentKontroler pacijentKontroler = new PacijentKontroler();
            //TODO OVO BI BILO LEPO DA SE URADI MALO ELEGANTNIJE
            foreach (Pacijent p in pacijentKontroler.GetSviPacijenti())
            {
                if (txtJMBG.Text.Equals(p.Jmbg))
                {
                    txtIme.Text = p.Ime;
                    txtPrz.Text = p.Prezime;
                    btnOperacija.IsEnabled = true;
                    btnPregled.IsEnabled = true;
                    btnIzvestaj.IsEnabled = true;
                    btnHospitalizacija.IsEnabled = true;
                    nasao = true;
                }
                else
                {
                    Console.WriteLine(txtJMBG);
                    Console.WriteLine(p.Jmbg);
                    Console.WriteLine("Ovaj nije isti");
                }

                if (!nasao)
                {
                    btnOperacija.IsEnabled = false;
                    btnPregled.IsEnabled = false;
                    btnIzvestaj.IsEnabled = false;
                    btnHospitalizacija.IsEnabled = false;
                }

            }
            if (!nasao) 
            { 
                MessageBox.Show("Ne postoji pacijent sa unetim jmbg-om", "Probaj ponovo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            */
        }

        private void ButtonRaspored_Click(object sender, RoutedEventArgs e)
        {
            var raspored = new LekarRaspored(Sifra);

            this.NavigationService.Navigate(raspored);
        }

        private void odjavaClick(object sender, RoutedEventArgs e)
        {
            MainWindow prijava = new MainWindow();
            Application.Current.Windows[0].Close();
            prijava.ShowDialog();


        }

        private void ButtonIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            string jmbgPacijenta = txtJMBG.Text;
            LekarIzvestaj izvestaj = new LekarIzvestaj(jmbgPacijenta, Sifra);
            izvestaj.Show();

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public string Sifra { get; set; }

        private void btnKreirajHospitalizaciju(object sender, RoutedEventArgs e)
        {
            LekarKreiranjeHospitalizacije kreiranjeHospitalizacije = new LekarKreiranjeHospitalizacije(txtJMBG.Text);
            this.NavigationService.Navigate(kreiranjeHospitalizacije);
        }

        private void ButtonClick_ViseOPacijentu(object sender, RoutedEventArgs e)
        {
            PodaciOPacijentu podaci = new PodaciOPacijentu(txtJMBG.Text);
            NavigationService.Navigate(podaci);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }
    }




}
