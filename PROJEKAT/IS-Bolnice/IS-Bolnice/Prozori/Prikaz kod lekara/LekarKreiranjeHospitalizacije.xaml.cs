using IS_Bolnice.Repozitorijumi;
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
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Kontroleri.Ustanova;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarKreiranjeHospitalizacije.xaml
    /// </summary>
    public partial class LekarKreiranjeHospitalizacije : Page
    {
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        public LekarKreiranjeHospitalizacije(string sifra)
        {
            InitializeComponent();
            txtJMBG.Text = sifra;
            List<Soba> listaSvihSoba = bolnicaKontroler.GetSveSobeZaHospitalizaciju();
            
            datumKraja.SelectedDate = DateTime.Today.AddDays(1);
            listaSoba.ItemsSource = listaSvihSoba;
        }

        private void ButtonClick_Zakazi(object sender, RoutedEventArgs e)
        {
            if (listaSoba.SelectedIndex != -1)
            {
                MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite postali pacijenta na hospitalizaciju?", "Kreiranje hospitalizacije", "Da", "Ne", MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    Soba soba = (Soba) listaSoba.SelectedItem;
                    Hospitalizacija hos = new Hospitalizacija(txtJMBG.Text, soba.Id, datumKraja.SelectedDate.Value);

                    if (new HospitalizacijaKontroler().KreirajHospitalizaciju(hos))
                    {
                        MessageBox.Show("Pacijent poslan na hospitalizaciju", "Kreirana hospitalizacija",
                            MessageBoxButton.OK);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Nije moguće kreirati hospitalizaciju", "Nije kreirana hospitalizacija",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nije selektovana ni jedna soba", "Nije kreirana hospitalizacija",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClick_Odustani(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da napustite kreiranje hospitalizacije!", "Kreiranje hospitalizacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
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
