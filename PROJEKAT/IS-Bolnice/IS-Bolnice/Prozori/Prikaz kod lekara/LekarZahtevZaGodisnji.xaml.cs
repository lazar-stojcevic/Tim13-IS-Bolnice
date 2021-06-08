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
            if (pocetak.Text.Length < 2 || kraj.Text.Length < 2)
            {
                CustomMessageBox.ShowOK("Niste dobro popunili sva polja", "Zahtev za odmor", "Dobro", MessageBoxImage.Error);
                return;
            }

            DateTime startTime = (DateTime)pocetak.SelectedDate;
            DateTime endTime = (DateTime)kraj.SelectedDate;

            TimeSpan span = endTime.Subtract(startTime);

            if (span.Days > Int16.Parse(PreostaliDani.Text))
            {
                CustomMessageBox.ShowOK("Uneliste ste predugačak vremenski period!", "Zahtev za odmor", "Dobro", MessageBoxImage.Error);
                return;
            }


            if (span.Days < 0)
            {
                CustomMessageBox.ShowOK("Uneli ste negativan vremenski perion!", "Zahtev za odmor", "Dobro", MessageBoxImage.Error);
                return;
            }


            CustomMessageBox.ShowOK("Uspešno ste podneli zahtev za godišnji odmor", "Kreiran zahtev za odmor", "Dobro", MessageBoxImage.Information);
            NavigationService.GoBack();
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

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[#]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
