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
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Lekovi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarZahtevValidacije.xaml
    /// </summary>
    public partial class LekarZahtevValidacije : Page
    {
        ZahtevZaValidacijuLeka zahtev = new ZahtevZaValidacijuLeka(new Lek());
        private ZahtevZaValidacijuKontroler zahtevZaValidacijuKontroler = new ZahtevZaValidacijuKontroler();
        string sifra;
        public LekarZahtevValidacije(string sifraLeka, string sifraLekara)
        {
            sifra = sifraLekara;
            InitializeComponent();
            List<ZahtevZaValidacijuLeka> sviZahtevi = zahtevZaValidacijuKontroler.GetSviZaValidacijuLeka();
            foreach (ZahtevZaValidacijuLeka zahtevIter in sviZahtevi)
            {
                if (zahtevIter.Lek.Id.Equals(sifraLeka))
                {
                    zahtev = zahtevIter;
                    break;
                }   
            }

            zahtev.Id = zahtev.Lek.Id; 
            txtIme.Text = zahtev.Lek.Ime;
            txtGramaza.Text = zahtev.Lek.Opis;
            foreach (Sastojak sastojak in zahtev.Lek.Alergeni)
            {
                listSastojci.Items.Add(sastojak.Ime);
            }
            txtSifra.Text = zahtev.Lek.Id;

            listZamesnski.ItemsSource = zahtev.Lek.ZamenskiLekovi;

        }

        private void Button_ClickOdbij(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da odbijete ovaj lek?", "Odbijanje leka", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                LekarRazlogOdbijanjaLeka razlogOdbijanjaLeka = new LekarRazlogOdbijanjaLeka(zahtev);
                razlogOdbijanjaLeka.ShowDialog();
                LekarGlavniMeni meni = new LekarGlavniMeni(sifra);
                this.NavigationService.Navigate(meni);
            }
        }

        private void Button_Potvrdi(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da validirate ovaj lek", "Validacija leka", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                LekKontroler lekKontroler = new LekKontroler();
                lekKontroler.KreirajLek(zahtev.Lek);

                zahtevZaValidacijuKontroler.ObrisiZahtev(zahtev);

                LekarGlavniMeni meni = new LekarGlavniMeni(sifra);
                this.NavigationService.Navigate(meni);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
