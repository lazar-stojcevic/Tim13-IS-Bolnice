using IS_Bolnice.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarPrikazSvihHospitalizacija.xaml
    /// </summary>
    public partial class LekarPrikazSvihHospitalizacija : Page
    {
        private ObservableCollection<Hospitalizacija> sveHospitalizacije = new ObservableCollection<Hospitalizacija>();
        private HospitalizacijaKontroler hospitalizacijaKontroler = new HospitalizacijaKontroler();
        public LekarPrikazSvihHospitalizacija()
        {
            foreach (Hospitalizacija hospitalizacija in hospitalizacijaKontroler.GetSveHospitalizacije())
            {
                sveHospitalizacije.Add(hospitalizacija);
            }
            InitializeComponent();
            listaHospitalizacija.ItemsSource = sveHospitalizacije;
        }

        private void ButtonClick_Produzi(object sender, RoutedEventArgs e)
        {
            if (listaHospitalizacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nijedna hospitalizacija nije selektovana", "Ništa nije selektovano",
                    MessageBoxButton.OK);
                return;
            }
            LekarProduzenjeHospitalizacije produzenjeHospitalizacije = new LekarProduzenjeHospitalizacije(sveHospitalizacije.ElementAt(listaHospitalizacija.SelectedIndex));
            produzenjeHospitalizacije.Show();

        }

        private void ButtonClick_Otpusti(object sender, RoutedEventArgs e)
        {
            if (listaHospitalizacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nijedna hospitalizacija nije selektovana", "Ništa nije selektovano",
                    MessageBoxButton.OK);
                return;
            }
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otpustite pacijenta kući?!", "Otpuštanje sa hospitalizacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                hospitalizacijaKontroler.ObrisiHospitalizaciju(sveHospitalizacije.ElementAt(listaHospitalizacija.SelectedIndex));

                sveHospitalizacije.RemoveAt(listaHospitalizacija.SelectedIndex);
            }

        }

        private void ButtonClick_Nazad(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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
