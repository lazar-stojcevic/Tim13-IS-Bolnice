using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prikaz_za_upravnika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for NalogPage.xaml
    /// </summary>
    public partial class NalogPage : Page
    {
        Upravnik ovajUpravnik = new Upravnik();

        public NalogPage(Upravnik upravnik)
        {
            InitializeComponent();
            ovajUpravnik = upravnik;
            Refresh();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow logOut = new MainWindow();
            logOut.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }

        private void Refresh() {
            textBoxKorisnickoIme.Text = ovajUpravnik.KorisnickoIme;
            textBoxIme.Text = ovajUpravnik.Ime;
            textBoxPrezime.Text = ovajUpravnik.Prezime;
            textBoxBroj.Text = ovajUpravnik.BrojTelefona;
            textBoxMail.Text = ovajUpravnik.EMail;

        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            Page page = new ObavestenjaPage();
            this.NavigationService.Navigate(page);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ovajUpravnik.KorisnickoIme = textBoxKorisnickoIme.Text;
            ovajUpravnik.Ime = textBoxIme.Text;
            ovajUpravnik.Prezime = textBoxPrezime.Text;
            ovajUpravnik.BrojTelefona = textBoxBroj.Text;
            ovajUpravnik.EMail = textBoxMail.Text;
            UpravnikKontroler kontroler = new UpravnikKontroler();
            kontroler.Izmeni(ovajUpravnik);
            Refresh();
        }
    }
}
