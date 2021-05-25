using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarZahtevZaGodisnji.xaml
    /// </summary>
    public partial class LekarZahtevZaGodisnji : Page
    {
        public LekarZahtevZaGodisnji()
        {
            InitializeComponent();
            PreostaliDani.Text = "14";
        }

        private void ButtonClick_Povrdi(object sender, RoutedEventArgs e)
        {
            DateTime startTime = pocetak.DisplayDate;
            DateTime endTime = kraj.DisplayDate;

            TimeSpan span = endTime.Subtract(startTime);

            if (span.Days > Int16.Parse(PreostaliDani.Text))
            {
                CustomMessageBox.ShowOK("Uneliste ste predugačak vremenski period!", "Zahtev za odmor", "Dobro", MessageBoxImage.Error);
            }


            if (span.Days < 0)
            {
                CustomMessageBox.ShowOK("Uneli ste negativan vremenski perion!", "Zahtev za odmor", "Dobro", MessageBoxImage.Error);
            }

        }

        private void ButtonClick_Odustani(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da odustanete od podnošenja zahteva za odmor?", "Zahtev za odmor", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
